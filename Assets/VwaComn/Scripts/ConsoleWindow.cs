/*
    Virtual World Arcade Common Scripts

    SupportLogger.cs

    Source:    https://garry.tv/2014/04/23/unity-batchmode-console/

    fix it with this
    https://gist.github.com/bboyle1234/a225218cf4a6825c058c
    http://stackoverflow.com/questions/474679/capture-console-exit-c-sharp
    http://stackoverflow.com/questions/11959643/why-does-closing-a-console-that-was-started-with-allocconsole-cause-my-whole-app
    http://stackoverflow.com/questions/533491/how-to-use-setconsolehandler-to-block-exit-calls

*/


using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;




namespace Windows
{
    /// <summary>
    /// Creates a console window that actually works in Unity
    /// You should add a script that redirects output using Console.Write to write to it.
    /// </summary>
    public class ConsoleWindow
    {
        TextWriter oldOutput;
        static Boolean readyToExit = false;
        static void OnProcessExit(object sender, EventArgs e)
        {
            readyToExit = true;
        }
        public void Initialize()
        {
            AppDomain.CurrentDomain.ProcessExit +=OnProcessExit;

            //
            // Attach to any existing consoles we have
            // failing that, create a new one.
            //
            if (!AttachConsole(0x0ffffffff))
            {
                AllocConsole();
            }


            /*
             Fix for application termination from console closing
             Must be called AFTER obtaining console
            */
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            oldOutput = Console.Out;

            try
            {
                IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                Microsoft.Win32.SafeHandles.SafeFileHandle safeFileHandle = new Microsoft.Win32.SafeHandles.SafeFileHandle(stdHandle, true);
                FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                System.Text.Encoding encoding = System.Text.Encoding.ASCII;
                StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
            catch (System.Exception e)
            {
                Debug.Log("Couldn't redirect output: " + e.Message);
            }
        }

        public void Shutdown()
        {   
            if(oldOutput != null)
                Console.SetOut(oldOutput);
            FreeConsole();
        }

        public void SetTitle(string strName)
        {
            SetConsoleTitle(strName);
        }

        private const int STD_OUTPUT_HANDLE = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleTitle(string lpConsoleTitle);

        /* Fix for preventing console window closing from terminating 
            any associated processes, Unity Editor, Game itself etc..
            Do not accidentally close command line in Unity editor! or else it'll close the program entirely!
            There is no way around the Windows autoclose timeout once the X is selected
            nor will attempting to hide
        */
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT: //Hack with Freeconsole and ExitThread, but all subsequent console will fail afterwards
                default:
                    Application.Quit(); //We cannot prevent the application from being ended, so at least end it gracefully as if closing the unity game window
                    System.Threading.Thread.Sleep(100); //Necessary hack for Photon/Unity to finish quitting?? not sure if asynchronous
                    return true;
            }
        }


    }
}

