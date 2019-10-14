using CefSharp;

namespace QGISerWPF
{
    public class QGISerSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "Map";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new QGISerSchemeHandler();
        }
    }
}
