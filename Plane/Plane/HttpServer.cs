using System.Net;
using Plane.Handler;

namespace HttpServer;

public class HttpServer
{
    private HttpListener _listener = new();
    private CancellationTokenSource cts = new();
    private ControllerHandler _controllerHandler = new ControllerHandler();

    public HttpServer()
    {
        _listener.Prefixes.Add($"{"http://127.0.0.1"}:{"2323"}/");
        Console.WriteLine($"Server has been started: {"http://127.0.0.1"}:{"2323"}/");
    }
    
    public async Task StartAsync()
    {
        var token = cts.Token;
        await Task.Run(async () => { await Run(token); });
    }

    private async Task Run(CancellationToken token)
    {
        _listener.Start();
        Task.Run(ProcessCallback);

        try
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                
                var context = await _listener.GetContextAsync();
                _controllerHandler.HandleRequest(context);
            }
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _listener.Close();
            ((IDisposable)_listener).Dispose();
            Console.WriteLine("Server has been stopped.");   
        }
    }

    private void ProcessCallback()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (input != "stop") continue;
            cts.Cancel();
            break;
        }
    }
}