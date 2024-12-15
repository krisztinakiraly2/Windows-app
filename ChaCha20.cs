using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsApp
{
    using System;
    using System.Security.Cryptography;

    public class ChaCha20
    {
        private static readonly byte[] ChaCha20Constant = "expand 32-byte k"u8.ToArray();

        private static uint RotateLeft(uint value, int bits)
        {
            return (value << bits) | (value >> (32 - bits));
        }

        private static void QuarterRound(ref uint a, ref uint b, ref uint c, ref uint d)
        {
            a += b; d ^= a; d = RotateLeft(d, 16);
            c += d; b ^= c; b = RotateLeft(b, 12);
            a += b; d ^= a; d = RotateLeft(d, 8);
            c += d; b ^= c; b = RotateLeft(b, 7);
        }

        private static void GenerateBlock(uint[] state, byte[] keystream)
        {
            uint[] workingState = new uint[16];
            Array.Copy(state, workingState, 16);

            for (int i = 0; i < 10; i++)
            {
                QuarterRound(ref workingState[0], ref workingState[4], ref workingState[8], ref workingState[12]);
                QuarterRound(ref workingState[1], ref workingState[5], ref workingState[9], ref workingState[13]);
                QuarterRound(ref workingState[2], ref workingState[6], ref workingState[10], ref workingState[14]);
                QuarterRound(ref workingState[3], ref workingState[7], ref workingState[11], ref workingState[15]);
                QuarterRound(ref workingState[0], ref workingState[5], ref workingState[10], ref workingState[15]);
                QuarterRound(ref workingState[1], ref workingState[6], ref workingState[11], ref workingState[12]);
                QuarterRound(ref workingState[2], ref workingState[7], ref workingState[8], ref workingState[13]);
                QuarterRound(ref workingState[3], ref workingState[4], ref workingState[9], ref workingState[14]);
            }

            for (int i = 0; i < 16; i++)
            {
                workingState[i] += state[i];
            }

            Buffer.BlockCopy(workingState, 0, keystream, 0, 64);
        }

        private static void InitState(byte[] key, byte[] nonce, int counter, uint[] state)
        {
            state[0] = BitConverter.ToUInt32(ChaCha20Constant, 0);
            state[1] = BitConverter.ToUInt32(ChaCha20Constant, 4);
            state[2] = BitConverter.ToUInt32(ChaCha20Constant, 8);
            state[3] = BitConverter.ToUInt32(ChaCha20Constant, 12);

            for (int i = 0; i < 8; i++)
            {
                state[4 + i] = BitConverter.ToUInt32(key, i * 4);
            }

            state[12] = (uint)counter;
            state[13] = BitConverter.ToUInt32(nonce, 0);
            state[14] = BitConverter.ToUInt32(nonce, 4);
            state[15] = BitConverter.ToUInt32(nonce, 8);
        }

        public static byte[] EncryptDecrypt(byte[] key, byte[] nonce, int counter, byte[] data)
        {
            uint[] state = new uint[16];
            byte[] keystream = new byte[64];

            InitState(key, nonce, counter, state);

            byte[] result = new byte[data.Length];

            for (int i = 0; i < data.Length; i += 64)
            {
                GenerateBlock(state, keystream);

                for (int j = 0; j < 64 && (i + j) < data.Length; j++)
                {
                    result[i + j] = (byte)(data[i + j] ^ keystream[j]);
                }

                state[12]++;
                if (state[12] == 0)
                {
                    state[13]++;
                    if (state[13] == 0)
                    {
                        throw new InvalidOperationException("Counter overflow in ChaCha20.");
                    }
                }
            }

            return result;
        }
    }

}
