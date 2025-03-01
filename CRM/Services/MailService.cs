using System.Net;
using System.Net.Mail;

namespace CRM.Services;

public class MailService
{
   private readonly IConfiguration _configuration;

   public MailService(IConfiguration configuration)
   {
      _configuration = configuration;
   }
   public string SendMail(string to, string body, string title )
   {
      using (var smtp = new SmtpClient(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"])))
      {
         smtp.Credentials = new System.Net.NetworkCredential(
            _configuration["Smtp:Username"],
            _configuration["Smtp:Password"]
         );
         smtp.EnableSsl = true;
         smtp.UseDefaultCredentials = false;         
         var mail = new MailMessage
         {
            From = new MailAddress(_configuration["Smtp:From"]),
            Subject = title,
            Body = body,
            IsBodyHtml = true
         };
         mail.To.Add(to);
         smtp.Send(mail);
      }


      return "Mail enviado";
   }
}