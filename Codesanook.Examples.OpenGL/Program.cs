using OpenTK;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Codesanook.Examples.OpenGL
{
    public class Program
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);

            // Show
            //ShowWindow(handle, SW_SHOW);

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            var window = new GameWindow(500, 500);
            //window.Run(1.0/60.0);//60 FPS
            var game = new Game(window);
        }

        static void GameLoop(double elapsedTime)
        {
            // GameCode goes here
            // GetInput
            // Process
            // Render
            Console.WriteLine("loop");
        }
    }
}
