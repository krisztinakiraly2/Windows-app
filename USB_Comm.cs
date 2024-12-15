using System.IO.Ports;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WindowsApp
{
    class USB_Comm
    {
        SerialPort serial_port;
        int num=1;
        int i = 0;
        byte[] stm32_public_key_bytes = new byte[64];

        IAsymmetricCipherKeyPairGenerator generator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
        X9ECParameters ecParams = ECNamedCurveTable.GetByName("secp256r1");
        ECDomainParameters domain_params;
        AsymmetricCipherKeyPair key_pair;
        ECPublicKeyParameters my_public_key;
        byte[] my_public_key_bytes;
        ECPoint stm32_point;
        ECPublicKeyParameters stm32_public_key;
        IBasicAgreement agreement;
        BigInteger shared_secret;
        byte[] shared_secret_bytes;

        int first = 1;

        byte[] nonce =
        [
            0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x4A,
            0x00, 0x00, 0x00, 0x00
        ];

        int counter = 1;

        const string SET_UP_ENCRYPTION = "1";
        const string COM_PORT = "COM10";

        static bool isComPortOpen = false;
        public static bool IsComPortOpen
        {
            get { return isComPortOpen; }
        }

        InputLanguage currentLanguage;

        ManualResetEvent waitHandle = new ManualResetEvent(false);
        const string hu = "hu-HU";
        const string en = "en-GB";
        string name;

        public USB_Comm() 
        {
            currentLanguage = InputLanguage.CurrentInputLanguage;
            name = currentLanguage.Culture.Name;

            domain_params = new ECDomainParameters(ecParams);
            generator.Init(new ECKeyGenerationParameters(domain_params, new SecureRandom()));
            key_pair = generator.GenerateKeyPair();
            my_public_key = (ECPublicKeyParameters)key_pair.Public;
            my_public_key_bytes = my_public_key.Q.GetEncoded(false);

            waitHandle = new ManualResetEvent(false);

            Thread serialThread = new Thread(InitializeSerialCommunication);
            serialThread.Start();
        }

        private void InitializeSerialCommunication()
        {
            try
            {
                serial_port = new SerialPort(COM_PORT, 19200, Parity.None, 8, StopBits.One);
                serial_port.Handshake = Handshake.None;

                var init = new SerialDataReceivedEventHandler(myDataReceived);
                var req = new SerialDataReceivedEventHandler(processRequest);
                serial_port.DataReceived += init;

                serial_port.Open();

                if (serial_port.IsOpen)
                {
                    serial_port.Write(SET_UP_ENCRYPTION);
                }
                else
                    Console.WriteLine("Serial is not open :(");

                waitHandle.WaitOne();
                waitHandle.Reset();

                string ms = "2 " + (name.Contains("hu") ? hu : en);
                serial_port.Write(ms);

                isComPortOpen = true;

                serial_port.DataReceived -= init;
                serial_port.DataReceived += req;

                waitHandle.WaitOne();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
        }

        public void processRequest(object sender, SerialDataReceivedEventArgs e)
        {
            int bytes_to_read = serial_port.BytesToRead;
            byte[] messageBytes = new byte[bytes_to_read];
            bool login_req = false;
            int bytesRead = serial_port.Read(messageBytes, 0, messageBytes.Length);

            if (bytesRead > 0)
            {
                byte[] decrypted = ChaCha20.EncryptDecrypt(shared_secret_bytes, nonce, counter++, messageBytes);
                string res_decrypted = System.Text.Encoding.UTF8.GetString(decrypted);
                if (res_decrypted.Contains("Login req"))
                    login_req = true;
                else
                    --counter;
            }

            if(login_req)
            {
                if(PWMan.SelectedEmailOrUsername != "" && PWMan.SelectedPassword != "")
                {
                    string emailOrUsername = PWMan.SelectedEmailOrUsername;
                    string password = PWMan.SelectedPassword;

                    byte[] emailOrUsernameArray = Encoding.UTF8.GetBytes(emailOrUsername);
                    byte[] passwordArray = Encoding.UTF8.GetBytes(password);

                    byte[] emailOrUsernameEncrypted = ChaCha20.EncryptDecrypt(shared_secret_bytes, nonce, counter++, emailOrUsernameArray);
                    byte[] passwordEncrypted = ChaCha20.EncryptDecrypt(shared_secret_bytes, nonce, counter++, passwordArray);

                    serial_port.Write(emailOrUsernameEncrypted, 0, emailOrUsernameEncrypted.Length);
                    Thread.Sleep(100);
                    serial_port.Write(passwordEncrypted, 0, passwordEncrypted.Length); 
                }
                else
                {
                    serial_port.Write("\0");
                }
                login_req = false;
            }
        }

        public void myDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!serial_port.IsOpen)
            {
                Console.WriteLine("Serial port is not open.");
                return;
            }

            if (serial_port.BytesToRead < 1)
                return;
            
            string d = new string("");
            byte[] buffer = new byte[100];
            int bytesRead = 0;

            if (num == 2)
            {
                bytesRead = serial_port.Read(buffer, 0, buffer.Length);
            }
            else
            {
                d = serial_port.ReadLine();

                if (d.Length > 1)
                    d = d.Substring(1);
            }

            switch(num)
            {
                case 1: 
                    try { stm32_public_key_bytes[i++] = Byte.Parse(d); }
                    catch (FormatException) { Console.WriteLine($"Unable to parse '{d}'"); }

                    if (i > 63)
                    {
                        BigInteger x = new BigInteger(1, stm32_public_key_bytes[..32]);
                        BigInteger y = new BigInteger(1, stm32_public_key_bytes[32..]);
                        stm32_point = ecParams.Curve.CreatePoint(x, y);
                        stm32_public_key = new ECPublicKeyParameters(stm32_point, domain_params);

                        agreement = AgreementUtilities.GetBasicAgreement("ECDH");
                        agreement.Init(key_pair.Private);
                        shared_secret = agreement.CalculateAgreement(stm32_public_key);
                        shared_secret_bytes = shared_secret.ToByteArrayUnsigned();

                        byte[] short_pk = new byte[64];
                        for (int i = 0; i < short_pk.Length; i++)
                            short_pk[i] = my_public_key_bytes[i+1];
                        serial_port.Write(short_pk, 0, short_pk.Length);

                        i = 0;
                        ++num;
                    }

                    break;

                case 2:
                    if (bytesRead > 1)
                    {
                        byte[] messageBytes = new byte[8];
                        int k = 0; first = 0;

                        foreach (var b in buffer.Take(bytesRead))
                        {
                            if (b==0 && first==0)
                            {
                                ++first;
                                continue;
                            }
                            else
                                if (k < 8)
                                    messageBytes[k++] = b;
                                else
                                    break;
                        }

                        bool send = false;
                        byte[] decrypted = ChaCha20.EncryptDecrypt(shared_secret_bytes, nonce, counter++, messageBytes);
                        string res_decrypted = System.Text.Encoding.UTF8.GetString(decrypted);
                        if(res_decrypted.Contains("Comm set"))
                            send = true;

                        byte[] encrypted = ChaCha20.EncryptDecrypt(shared_secret_bytes, nonce, counter++, decrypted);

                        if (send)
                        {
                            serial_port.Write(encrypted, 0, encrypted.Length);
                            ++num;
                        }
                        else
                            counter -= 2;
                    }

                    break;


                case 3:
                    if (d.Contains("No"))
                    {
                        num = 1;
                        first = 1;
                        counter = 1;
                    }
                    else
                        if (d.Contains("Yes"))
                        {
                            MessageBox.Show("Setting up connection with the device has been successful");
                            waitHandle.Set();
                        }
                    break;
            }
        }

        public void Program_FormClosed(object sender, FormClosedEventArgs e)
        {
            serial_port.Close();
        }
    }
}
