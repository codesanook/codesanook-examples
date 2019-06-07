using React;
using System.Web.Hosting;

namespace ReactServerSideRendering.Helpers
{
	/// <summary>
	/// Handles file system functionality, such as reading files. Maps all paths from 
	/// application-relative (~/...) to full paths using ASP.NET's MapPath method.
	/// </summary>
	public class AspNetFileSystem : FileSystemBase
	{
		/// <summary>
		/// Converts a path from an application relative path (~/...) to a full filesystem path
		/// </summary>
		/// <param name="relativePath">App-relative path of the file</param>
		/// <returns>Full path of the file</returns>
		public override string MapPath(string relativePath)
		{
			return HostingEnvironment.MapPath(relativePath);
		}
	}
}
