using Windows.Devices.Pwm;
using WindowsApp;

namespace WindowsApp
{
    internal static class Program
    {
        static string baseF = AppDomain.CurrentDomain.BaseDirectory;
        static string d1 = Directory.GetParent(Directory.GetParent(Directory.GetParent(baseF).FullName).FullName).FullName;

        public static string projectFolderPath = Directory.GetParent(d1).FullName;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            USB_Comm usbComm = new USB_Comm();
            _ = new App(usbComm);
        }
    }
}