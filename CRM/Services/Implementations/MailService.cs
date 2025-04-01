using System.Net;
using System.Net.Mail;
using CRM.Models.Dto;
using CRM.Services.Contracts;

namespace CRM.Services.Implementations
{
   
   public class MailService:IMailService
   {
      private readonly IConfiguration _configuration;

      public MailService(IConfiguration configuration)
      {
         _configuration = configuration;
      }
      public string SendRegisterMail(UserRegDto newUser)
      {
         using (var smtp = new SmtpClient(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"])))
         {
            smtp.Credentials = new System.Net.NetworkCredential(
               _configuration["Smtp:Username"],
               _configuration["Smtp:Password"]
            );
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;

            var title = $"¡Hola {newUser.Username}! Bienvenido a tu CRM";
            var body = "Ya puedes usar la aplicación";
            var mail = new MailMessage
            {
               From = new MailAddress(_configuration["Smtp:From"]),
               Subject = title,
               Body = body,
               IsBodyHtml = true
            };
            mail.To.Add(newUser.Email);
            smtp.Send(mail);
         }
         return "Mail enviado";
      }

      public string SendNewPasswordMail(string username, string email, string newPassword)
      {
         using (var smtp = new SmtpClient(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"])))
         {
            smtp.Credentials = new System.Net.NetworkCredential(
               _configuration["Smtp:Username"],
               _configuration["Smtp:Password"]
            );
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;

            var title = $"¡Hola {username}! Reestablecimiento de contraseña";
            var body =
               "Hola, te enviamos tu nueva contraseña de uso temporal para que puedas crear una nueva desde el portal.\n" +
               $"Usuario: {username} \n" +
               $"Contraseña: {newPassword} \n";
            
            var mail = new MailMessage
            {
               From = new MailAddress(_configuration["Smtp:From"]),
               Subject = title,
               Body = body,
               IsBodyHtml = true
            };
            mail.To.Add(email);
            smtp.Send(mail);
         }
         return "Mail enviado";
      }
   }
}

