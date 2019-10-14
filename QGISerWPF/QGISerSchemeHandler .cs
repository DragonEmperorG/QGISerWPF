using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QGISerWPF
{
    public class QGISerSchemeHandler : ResourceHandler
    {
        public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback)
        {
            var names = this.GetType().Assembly.GetManifestResourceNames();

            Console.WriteLine(names);

            Uri uri = new Uri(request.Url);
            String file = uri.Authority + uri.AbsolutePath; // 注：目录名需全为小写字母，否则将无法得到 Resource

            Assembly assembly = Assembly.GetExecutingAssembly();
            String resourcePath = assembly.GetName().Name + "." + file.Replace("/", ".");

            Task.Run(() =>
            {
                using (callback)
                {
                    if (assembly.GetManifestResourceInfo(resourcePath) != null)
                    {
                        Stream stream = assembly.GetManifestResourceStream(resourcePath);
                        string mimeType = "application/octet-stream";
                        switch (Path.GetExtension(file))
                        {
                            case ".html":
                                mimeType = "text/html";
                                break;
                            case ".js":
                                mimeType = "text/javascript";
                                break;
                            case ".css":
                                mimeType = "text/css";
                                break;
                            case ".png":
                                mimeType = "image/png";
                                break;
                            case ".appcache":
                                break;
                            case ".manifest":
                                mimeType = "text/cache-manifest";
                                break;
                        }

                        // Reset the stream position to 0 so the stream can be copied into the underlying unmanaged buffer
                        stream.Position = 0;
                        // Populate the response values - No longer need to implement GetResponseHeaders (unless you need to perform a redirect)
                        ResponseLength = stream.Length;
                        MimeType = mimeType;
                        StatusCode = (int) HttpStatusCode.OK;
                        Stream = stream;

                        callback.Continue();
                    }
                    else
                    {
                        callback.Cancel();
                    }
                }
            });

            return CefReturnValue.Continue;
        }
    }
}
