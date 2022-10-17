using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;


namespace Foody.Data.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendMail(EmailDto email)
    {
        var mail = new MimeMessage()
        {
            Subject = email.Subject,
            Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = email.Body
            },
        };
        

        mail.From.Add(MailboxAddress.Parse(email.From));
        mail.To.Add(MailboxAddress.Parse(email.To));

        var smtp = new SmtpClient();

        await smtp.ConnectAsync(_config["Mailtrap:Host"], _config.GetValue<int>("Mailtrap:Port"), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_config["Mailtrap:Username"], _config["Mailtrap:Password"]);
        await smtp.SendAsync(mail);
        await smtp.DisconnectAsync(true);
    }
}

