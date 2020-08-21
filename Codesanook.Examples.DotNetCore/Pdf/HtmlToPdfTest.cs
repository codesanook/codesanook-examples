using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharp;
using PdfSharp.Drawing;
using System.IO;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using System.Threading;
using System.Drawing;

namespace Codesanook.Examples.DotNetCore.Pdf
{
    public class HtmlToPdfTest
    {
        /// <summary>
        /// Cache for resource images
        /// </summary>
        private static readonly Dictionary<string, Image> _imageCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        [Fact]
        public void GeneratePdf_ValidHtmlContent_ReturnPdfDocument()
        {
            const string htmlContent = @"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <link rel=""Stylesheet"" href=""StyleSheet"" />
                    <title>F#</title>
                </head>
                <body>
                    <h1>F Sharp (programming language) ภาษา F#</h1>
                    <p>
                        F# is a mature, open source, cross-platform, functional-first programming language. 
                        It empowers users and organizations to tackle complex computing problems with simple, 
                        maintainable and robust code.
                    </p>
                </body>
                </html>";
            /*
            By default, A runtime throws a NotSupportedException 
            "No data is available for encoding 1252." on .NET Core.
            To fix, add a dependency to the package System.Text.Encoding.CodePages 
            and then add code to register the code page provider during application initialization (f.ex in Startup.cs):

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            This is required to parse strings in binary BIFF2-5 Excel documents encoded with DOS-era code pages. 
            These encodings are registered by default in the full .NET Framework, but not on .NET Core.
            */
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var config = new PdfGenerateConfig { PageSize = PageSize.A4 };
            config.SetMargins(20);

            var document = PdfGenerator.GeneratePdf(
                htmlContent,
                config,
                null,
                OnStylesheetLoad,
                OnImageLoadPdfSharp
            );

            var tempFile = Path.GetFileNameWithoutExtension(
                $"{Path.GetTempFileName()}.pdf"
            );
            document.Save(tempFile);
        }

        /// <summary>
        /// Handle stylesheet resolve.
        /// </summary>
        private static void OnStylesheetLoad(object sender, HtmlStylesheetLoadEventArgs e)
        {
            var stylesheet = GetStylesheet(e.Src);
            if (stylesheet != null)
                e.SetStyleSheet = stylesheet;
        }

        /// <summary>
        /// Get stylesheet by given key.
        /// </summary>
        private static string GetStylesheet(string src)
        {
            if (src == "StyleSheet")
            {
                return @"h1, h2, h3 { color: navy; font-weight:normal; }
                    h1 { margin-bottom: .47em }
                    h2 { margin-bottom: .3em }
                    h3 { margin-bottom: .4em }
                    ul { margin-top: .5em }
                    ul li {margin: .25em}
                    body { font:10pt Tahoma }
		            pre  { border:solid 1px gray; background-color:#eee; padding:1em }
                    a:link { text-decoration: none; }
                    a:hover { text-decoration: underline; }
                    .gray    { color:gray; }
                    .example { background-color:#efefef; corner-radius:5px; padding:0.5em; }
                    .whitehole { background-color:white; corner-radius:10px; padding:15px; }
                    .caption { font-size: 1.1em }
                    .comment { color: green; margin-bottom: 5px; margin-left: 3px; }
                    .comment2 { color: green; }";
            }
            return null;
        }

        /// <summary>
        /// On image load in renderer set the image by event async.
        /// </summary>
        private static void OnImageLoadPdfSharp(object sender, HtmlImageLoadEventArgs e) => ImageLoad(e, true);

        /// <summary>
        /// On image load in renderer set the image by event async.
        /// </summary>
        private static void ImageLoad(HtmlImageLoadEventArgs e, bool pdfSharp)
        {
            var img = TryLoadResourceImage(e.Src);
            XImage xImg = null;

            if (img != null)
            {
                using var ms = new MemoryStream();
                img.Save(ms, img.RawFormat);
                xImg = img != null ? XImage.FromStream(ms) : null;
            }

            object imgObj;
            if (pdfSharp)
                imgObj = xImg;
            else
                imgObj = img;

            if (!e.Handled && e.Attributes != null)
            {
                if (e.Attributes.ContainsKey("byevent"))
                {
                    int delay;
                    if (Int32.TryParse(e.Attributes["byevent"], out delay))
                    {
                        e.Handled = true;
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            Thread.Sleep(delay);
                            e.Callback("https://fbcdn-sphotos-a-a.akamaihd.net/hphotos-ak-snc7/c0.44.403.403/p403x403/318890_10151195988833836_1081776452_n.jpg");
                        });
                        return;
                    }
                    else
                    {
                        e.Callback("http://sphotos-a.xx.fbcdn.net/hphotos-ash4/c22.0.403.403/p403x403/263440_10152243591765596_773620816_n.jpg");
                        return;
                    }
                }
                else if (e.Attributes.ContainsKey("byrect"))
                {
                    var split = e.Attributes["byrect"].Split(',');
                    var rect = new Rectangle(Int32.Parse(split[0]), Int32.Parse(split[1]), Int32.Parse(split[2]), Int32.Parse(split[3]));
                    e.Callback(imgObj ?? TryLoadResourceImage("htmlicon"), rect.X, rect.Y, rect.Width, rect.Height);
                    return;
                }
            }

            if (img != null)
                e.Callback(imgObj);
        }

        /// <summary>
        /// Get image by resource key.
        /// </summary>
        public static Image TryLoadResourceImage(string src)
        {
            if (!_imageCache.TryGetValue(src, out Image image))
            {
                var imageStream = GetImageStream(src);
                if (imageStream != null)
                {
                    image = Image.FromStream(imageStream);
                    _imageCache[src] = image;
                }
            }
            return image;
        }


        /// <summary>
        /// Get image by resource key.
        /// </summary>
        public static Stream GetImageStream(string src)
        {
            switch (src.ToLower())
            {
                case "htmlicon":
                    return Resources.Html32;
                case "staricon":
                    return Resources.Favorites32;
                case "fonticon":
                    return Resources.Font32;
                case "commenticon":
                    return Resources.Comment16;
                case "imageicon":
                    return Resources.Image32;
                case "methodicon":
                    return Resources.Method16;
                case "propertyicon":
                    return Resources.Property16;
                case "eventicon":
                    return Resources.Event16;
                default:
                    break;
            }
            return null;
        }
    }
}
