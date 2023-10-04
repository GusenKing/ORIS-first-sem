using System.Net;

namespace MyHTTPServer.Handlers;

public class StaticFilesHandler : Handler
{
    public override HttpListenerResponse HandleRequest(HttpListenerContext context)
    {
        var absolutePath = context.Request.Url!.AbsolutePath;
        //TODO написать нормальную логику определения, что запрашивается файл
        
        if (absolutePath.EndsWith(".html"))
        {
            // завершение выполнения запроса;
        }
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}