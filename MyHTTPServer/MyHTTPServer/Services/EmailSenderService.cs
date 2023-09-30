using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace MyHTTPServer.Services;

public class EmailSenderService : IEmailSenderService
{
    public string? EmailSender { get; set; }
    
    public string? PasswordSender { get; set; }
    
    public string? FromName { get; set; }
    
    public string? SmtpServerHost { get; set; }
    
    public ushort SmtpServerPort { get; set; }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    { 
        MailAddress from = new MailAddress(EmailSender, FromName);
        MailAddress to = new MailAddress(toEmail);
        MailMessage mail = new MailMessage(from, to);
        mail.Subject = subject;
        mail.Body = message;

        Attachment data = new Attachment("MyHTTPServerArchived.zip", MediaTypeNames.Application.Zip);
        ContentDisposition disposition = data.ContentDisposition;
        disposition.FileName = $"ProjectArchived_{DateTime.Now.ToString()}";
        mail.Attachments.Add(data);
        
        SmtpClient smtp = new SmtpClient(SmtpServerHost, SmtpServerPort);
        smtp.Credentials = new NetworkCredential(EmailSender, PasswordSender);
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(mail);
        Console.WriteLine("Письмо отправлено");
    }
}