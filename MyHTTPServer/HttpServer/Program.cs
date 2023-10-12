using HttpServer;

class Program
{
    private static async Task Main()
    {
        var server = new HttpServer.HttpServer();
        await server.StartAsync();
    }
}