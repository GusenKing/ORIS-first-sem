using System.Net;
using System.Text;

namespace MyHTTPServer.Handlers;

public class StaticFilesHandler : Handler
{
    private static string _errorPage = @"<!DOCTYPE html>
    <html>
        <head>
            <meta charset='utf-8'>
            <title>Not Found</title>
        </head>
        <body>
            <h1>404 файл не найден</h1>
        </body>
    </html>";
    public override async void HandleRequest(HttpListenerContext context)
    {
        var absolutePath = context.Request.Url!.AbsolutePath;
        var config = ConfigLoader.Config;
        //TODO написать нормальную логику определения, что запрашивается файл
        var desiredPath = config.StaticFilesPath + absolutePath;
        byte[] contentBytes;
        var response = context.Response;
        Console.WriteLine(desiredPath);
        if (desiredPath == "static/")
            desiredPath += "index.html";
        if (desiredPath.Contains("."))
        {
            if (desiredPath.EndsWith(".html"))
            {
                if (File.Exists(desiredPath))
                    contentBytes = File.ReadAllBytes($"./{desiredPath}");
                else
                    contentBytes = Encoding.UTF8.GetBytes(_errorPage);
                response.ContentEncoding = Encoding.UTF8;
            }
            else if (desiredPath.EndsWith(".ico"))
                contentBytes = new byte[] {};
            else if (desiredPath.EndsWith(".svg"))
            {
                contentBytes = File.ReadAllBytes(desiredPath);
                response.ContentType = "image/svg+xml";
            }
            else
            {
                contentBytes = File.ReadAllBytes(desiredPath);
            }
            response.ContentLength64 = contentBytes.Length;
            using Stream output = response.OutputStream;
                        
            await output.WriteAsync(contentBytes);
            await output.FlushAsync();
        
            Console.WriteLine("Запрос обработан");
        }
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}