using System.Net;
using System.Text;
using MyHTTPServer.Handlers;
using MyHTTPServer.Services;

namespace MyHTTPServer;

public static class ServerManager
{
    public static void Run()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token = cancelTokenSource.Token;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        
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
                    
                    Handler staticFilesHandler = new StaticFilesHandler();
                    Handler controllerHandler = new ControllerHandler();
                    staticFilesHandler.Successor = controllerHandler;
                    staticFilesHandler.HandleRequest(context);
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