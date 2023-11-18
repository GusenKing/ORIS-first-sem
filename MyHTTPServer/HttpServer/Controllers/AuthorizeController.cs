using HttpServer.Attribuets;
using HttpServer.Services;

namespace HttpServer.Controllers;

[Controller("Authorize")]
public class AuthorizeController
{
    [Post("SendToEmail")]
    public void SendToEmail(string emailFromUser, string passwordFromUser)
    {
        new EmailSenderService().SendEmail(emailFromUser, passwordFromUser, $"Письмо от Эмина Саруханова от {DateTime.Now}");
        Console.WriteLine("Email has been sent.");
    }
}