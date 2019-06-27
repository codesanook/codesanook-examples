using System;
using System.IO;
using System.Text;
using System.Web;

namespace Codesanook.Examples.ReactServerSideRendering.Helpers
{
	/// <summary>
	/// IHtmlString or IHtmlString action wrapper implementation
	/// </summary>
	public class ActionHtmlString : IHtmlString
	{
		private readonly Action<TextWriter> _textWriter;

		/// <summary>
		/// Constructor IHtmlString or IHtmlString action wrapper implementation
		/// </summary>
		/// <param name="textWriter"></param>
		public ActionHtmlString(Action<TextWriter> textWriter)
		{
			_textWriter = textWriter;
		}

		[ThreadStatic]
		private static StringWriter _sharedStringWriter;

		/// <summary>Returns an HTML-encoded string.</summary>
		/// <returns>An HTML-encoded string.</returns>
		public string ToHtmlString()
		{
			var stringWriter = _sharedStringWriter;
			if (stringWriter != null)
			{
				stringWriter.GetStringBuilder().Clear();
			}
			else
			{
				_sharedStringWriter = stringWriter = new StringWriter(new StringBuilder(128));
			}

			_textWriter(stringWriter);

			return stringWriter.ToString();
		}
	}
}
