using System.Net;

namespace MyHTTPServer.Handlers;

public class ControllerHandler : Handler
{
    public override HttpListenerResponse HandleRequest(HttpListenerContext context)
    {
        // некоторая обработка запроса
         
        if (condition==1)
        {
            // завершение выполнения запроса;
        }
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}