namespace Foody.Data.Services;

public interface IEmailService
{
    Task SendMail(EmailDto email);
}

