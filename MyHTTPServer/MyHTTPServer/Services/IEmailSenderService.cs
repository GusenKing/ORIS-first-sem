namespace MyHTTPServer.Services;

public interface IEmailSenderService
{ 
    Task SendEmailAsync(string to, string subject, string message);
}