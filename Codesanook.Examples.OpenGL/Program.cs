using System;
using System.Windows.Forms;

namespace Codesanook.Examples.OpenGL
{
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
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
