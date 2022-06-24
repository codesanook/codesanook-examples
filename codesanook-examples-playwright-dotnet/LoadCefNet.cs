using System.Diagnostics;
using System.Runtime.InteropServices;
using CefNet;
using Tmds.Utils;

namespace Codesanook.Examples.Playwright
{
    public class LoadCefNet
    {
        public static int Load(string[] args)
        {
            if (ExecFunction.IsExecFunctionCommand(args))
            {
                return ExecFunction.Program.Main(args);
            }

            var url = "https://jp.mercari.com/search?keyword=sony%20mz-ne810&order=desc&sort=created_time&status=on_sale";
            string cefPath = Path.Combine(GetProjectPath(), "cef");
            Console.WriteLine($"cefPath: {cefPath}");
            var path = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", Path.Combine(cefPath, "Release") + ";" + path);
            string libname = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "libcef.dll" : "libcef.so";
            Console.WriteLine($"path: {Environment.GetEnvironmentVariable("PATH")}");

            // This executable is called many times, because it
            // is also used for subprocesses. Let's print args
            // so we can differentiate between main process and
            // subprocesses. If one of the first args is for
            // example "--type=renderer" then it means that
            // this is a Renderer process. There may be more
            // subprocesses like GPU (--type=gpu-process) and
            // others. On Linux there are also special Zygote
            // processes.

            Console.Write("\nProcess args: ");
            if (args.Length == 0)
            {
                Console.Write("none (Main process)");
            }
            else
            {
                Console.WriteLine();
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Length > 128)
                        Console.WriteLine(args[i].Remove(128) + "...");
                    else
                        Console.WriteLine(args[i]);
                }
            }
            Console.Write("\n\n");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                && !args.Any(arg => arg.StartsWith("--type="))
                && !args.Contains("--no-zygote"))
            {
                Console.WriteLine("Please run with --no-zygote");
                return 0;
            }

            // CEF version
            if (args.Length == 0)
            {
                var version = new int[8];
                for (int i = 0; i < version.Length; i++)
                {
                    version[i] = CefApi.CefVersionInfo((CefVersionComponent)i);
                }
                Console.Write("CEF version: {0}\n", string.Join(".", version));
            }

            // Main args
            CefMainArgs main_args = CefMainArgs.CreateDefault();

            // Cef app
            var app = new CefApp();

            // Execute subprocesses. It is also possible to have
            // a separate executable for subprocesses by setting
            // cef_settings_t.browser_subprocess_path. In such
            // case cef_execute_process should not be called here.
            Console.Write("cef_execute_process, argc={0}\n", args.Length);
            int code = CefApi.ExecuteProcess(main_args, app, IntPtr.Zero);
            if (code >= 0)
            {
                main_args.Dispose();
                Environment.Exit(code);
            }

            // Application settings. It is mandatory to set the
            // "size" member.
            var settings = new CefSettings();
            //settings.MultiThreadedMessageLoop = true;
            settings.LocalesDirPath = Path.Combine(cefPath, "Resources", "locales");
            settings.ResourcesDirPath = Path.Combine(cefPath, "Resources");
            settings.LogSeverity = CefLogSeverity.Warning; // Show only warnings/errors
            settings.NoSandbox = true;
            settings.WindowlessRenderingEnabled = true;

            // Initialize CEF
            Console.Write("cef_initialize\n");
            CefApi.Initialize(main_args, settings, app, IntPtr.Zero);
            GC.KeepAlive(settings);
            main_args.Dispose();

            // Window info
            var windowInfo = new CefWindowInfo();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                windowInfo.SetAsPopup(IntPtr.Zero, "cefapi example");
            else
                windowInfo.WindowName = "cefapi example";


            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                NativeMethods.XSetErrorHandler(x11_error_handler);
                NativeMethods.XSetIOErrorHandler(x11_io_error_handler);
            }

            var browserSettings = new CefBrowserSettings();

            // Client handlers
            var client = new CefClientClass();


            // Create browser asynchronously. There is also a
            // synchronous version of this function available.
            Console.WriteLine("cef_browser_host_create_browser");
            bool cbok = CefApi.CreateBrowser(windowInfo, client, "https://yandex.com/", browserSettings, null, null);
            Console.WriteLine("CreateBrowser: {0}", cbok);
            windowInfo.Dispose();

            // Message loop. There is also cef_do_message_loop_work()
            // that allow for integrating with existing message loops.
            // On Windows for best performance you should set
            // cef_settings_t.multi_threaded_message_loop to true.
            // Note however that when you do that CEF UI thread is no
            // more application main thread and using CEF API is more
            // difficult and require using functions like cef_post_task
            // for running tasks on CEF UI thread.
            Console.WriteLine("cef_run_message_loop\n");
            CefApi.RunMessageLoop();

            // Release references to CefBrowser's (if any)
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Shutdown CEF
            Console.WriteLine("cef_shutdown\n");
            CefApi.Shutdown();


            GC.KeepAlive(client);
            return 0;
        }

        private static string GetProjectPath()
        {
            string projectPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            string rootPath = Path.GetPathRoot(projectPath);
            while (Path.GetFileName(projectPath) != "codesanook-examples-playwright-dotnet")
            {
                if (projectPath == rootPath)
                    throw new DirectoryNotFoundException("Could not find the project directory.");
                projectPath = Path.GetDirectoryName(projectPath);
            }
            return projectPath;
        }

        private unsafe static X11ErrorHandlerDelegate x11_error_handler = HandleX11Error;

        private unsafe static int HandleX11Error(IntPtr display, XErrorEvent* e)
        {
            Console.WriteLine("X11 error: type={0}, serial={1}, code={2}", e->type, e->serial, e->error_code);
            return 0;
        }

        private static X11IOErrorHandlerDelegate x11_io_error_handler = HandleX11IOError;


        private static int HandleX11IOError(IntPtr display)
        {
            return 0;
        }


        sealed class CefClientClass : CefClient
        {
            private CefLifeSpanHandlerClass CefLifeSpan = new CefLifeSpanHandlerClass();

            protected override CefLifeSpanHandler GetLifeSpanHandler()
            {
                return CefLifeSpan;
            }
        }

        sealed class CefLifeSpanHandlerClass : CefLifeSpanHandler
        {
            protected override void OnBeforeClose(CefBrowser browser)
            {
                // TODO: Check how many browsers do exist and quit message
                //       loop only when last browser is closed. Otherwise
                //       closing a popup window will exit app while main
                //       window shouldn't be closed.
                CefApi.QuitMessageLoop();
            }

        }
    }


}
