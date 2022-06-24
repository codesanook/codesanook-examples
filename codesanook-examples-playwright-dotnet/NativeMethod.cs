using System;
using System.Runtime.InteropServices;

namespace Codesanook.Examples.Playwright
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int X11ErrorHandlerDelegate(IntPtr display, XErrorEvent* @event);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int X11IOErrorHandlerDelegate(IntPtr display);

	public static class NativeMethods
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll", EntryPoint = "LoadLibraryW", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr LoadLibraryW(string filename);

		[DllImport("libdl", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		private static extern IntPtr dlopen (string path, int mode);

		public static IntPtr LoadLibrary(string filename)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				return LoadLibraryW(filename);
			return dlopen(filename, 2);
		}

		[DllImport("X11", CallingConvention = CallingConvention.Cdecl)]
		public static extern int XSetErrorHandler(X11ErrorHandlerDelegate callback);

		[DllImport("X11", CallingConvention = CallingConvention.Cdecl)]
		public static extern int XSetIOErrorHandler(X11IOErrorHandlerDelegate callback);

	}

	public struct XErrorEvent
	{
		public int type;
		public IntPtr display;
		public long serial; // serial number of failed request
		public byte error_code; // error code of failed request
		public byte request_code; // Major op-code of failed request
		public byte minor_code; // Minor op-code of failed request
		public IntPtr resourceid; // resource id
	}
}
