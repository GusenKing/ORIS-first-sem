using System.Net;

namespace MyHTTPServer.Handlers;

public abstract class Handler
{
    public Handler Successor { get; set; }
    public abstract HttpListenerResponse HandleRequest(HttpListenerContext context);
}