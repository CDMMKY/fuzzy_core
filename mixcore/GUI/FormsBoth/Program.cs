using System;
using System.Windows.Forms;
using Mix_core.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace Mix_core
{

  


    static class Program
    {
      

    
        
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Timers.Timer noSleep_the_sun_is_high = new System.Timers.Timer(90000);
            noSleep_the_sun_is_high.Elapsed += new System.Timers.ElapsedEventHandler(noSleep_the_sun_is_high_Elapsed);
            noSleep_the_sun_is_high.Start();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            System.Diagnostics.Contracts.Contract.ContractFailed += ContractFailed; 


            Application.Run(new Start_F());
        }

        private static void ContractFailed(object sender, System.Diagnostics.Contracts.ContractFailedEventArgs e)
        {
            Console.WriteLine(e.Condition);
            Console.WriteLine(e.Message);

            e.SetHandled();

            var exx = e.OriginalException;
            while (exx != null)
            {
                Console.WriteLine(exx.Message);
                Console.WriteLine(exx.Source);
                Console.WriteLine(exx.TargetSite);
                exx = exx.InnerException;
            }
        }

        static void noSleep_the_sun_is_high_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
               NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_CONTINUOUS | NativeMethods.EXECUTION_STATE.ES_DISPLAY_REQUIRED | NativeMethods.EXECUTION_STATE.ES_SYSTEM_REQUIRED);
                uint time_to_sleep =0;
                NativeMethods.SystemParametersInfo(NativeMethods.SPI.SPI_GETSCREENSAVETIMEOUT, 0, ref time_to_sleep, NativeMethods.SPIF.None);
                NativeMethods.SystemParametersInfo(NativeMethods.SPI.SPI_GETSCREENSAVETIMEOUT, time_to_sleep, ref time_to_sleep, NativeMethods.SPIF.None);


            }
            catch (Exception ){ }
            }


        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
           // ConfigurationManager.AppSettings
            string MethodOInitPath = (new FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName + "\\Methods\\Init";
            string[] seekcode = args.Name.Split(',');
            string seekpart = seekcode[0];
            List<string> files = Directory.GetFiles(MethodOInitPath, seekpart + "*.dll").ToList();

            if (files.Count > 0)
            {

                return Assembly.LoadFile(files[0]);
            }
            string MethodOTunePath = (new FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName + "\\Methods\\Tune";

            files = Directory.GetFiles(MethodOTunePath, seekpart + "*.dll").ToList();
            if (files.Count > 0)
            {

                return Assembly.LoadFile(files[0]);
            }

            return null;
        }

        internal static class NativeMethods
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            static public extern uint SetThreadExecutionState(EXECUTION_STATE esFlags);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static public extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref uint pvParam, SPIF fWinIni);
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static public extern bool SystemParametersInfo(SPI uiAction, uint uiParam, UIntPtr pvParam, SPIF fWinIni);

            public enum EXECUTION_STATE : uint
            {
                ES_AWAYMODE_REQUIRED = 0x00000040,
                ES_CONTINUOUS = 0x80000000,
                ES_DISPLAY_REQUIRED = 0x00000002,
                ES_SYSTEM_REQUIRED = 0x00000001,
                ES_USER_PRESENT = 0x00000004
            }

            public enum SPI : uint
            {
                SPI_GETSCREENSAVEACTIVE = 0x0010,
                SPI_SETSCREENSAVEACTIVE = 0x0011,
                SPI_GETSCREENSAVETIMEOUT = 0x000E,
                SPI_SETSCREENSAVETIMEOUT = 0x000F

            }
            public enum SPIF : uint
            {
                None = 0x00
            }

        }



    }
}
