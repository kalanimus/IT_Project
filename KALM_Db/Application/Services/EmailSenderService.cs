using System.Net;
using System.Net.Mail;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;


namespace Application.Services;
public class EmailSenderService : IEmailSenderService{
  private readonly string smtpServer;
  private readonly int port;
  private readonly string smtpUsername;
  private readonly string smtpPassword;

  public EmailSenderService(IConfiguration configuration){
    smtpServer = configuration["EmailSettings:SmtpServer"];
    port = configuration.GetValue<int>("EmailSettings:port");
    smtpUsername = configuration["EmailSettings:Username"];
    smtpPassword = configuration["EmailSettings:Password"];
  }

  public async Task SendEmail(string email, int code){
    using (SmtpClient smtpClient = new SmtpClient(smtpServer, port)) {
      smtpClient.UseDefaultCredentials = false;
      smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
      smtpClient.EnableSsl = true;

      using (MailMessage mailMessage = new MailMessage()) {
        mailMessage.From = new MailAddress(smtpUsername);
        mailMessage.To.Add(email);
        mailMessage.Subject = "Проверочный код от KALM";
        mailMessage.Body = $"Ваш проверочный код: {code}";

        try {
          smtpClient.Send(mailMessage);
        } 
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
      }
    }
  }

}