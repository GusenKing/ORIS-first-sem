using System.Net;
using System.Net.Mail;
using System.Security;
using MyHTTPServer.Configuration;

namespace MyHTTPServer;

public class Server : IDisposable
{
    private CancellationTokenSource _cancellationTokenSource;
    private HttpListener _listener;
    private AppSettingConfig _config;

    public async void Start()
    {
        _listener.Start();
    }
    
    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }
    
    public void Dispose()
    {
        Stop();
    }


    
}