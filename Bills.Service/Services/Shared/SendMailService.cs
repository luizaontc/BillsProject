using Bills.Domain.Shared;
using Bills.Service.Interface.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Bills.Service.Services.Shared
{
    public class SendMailService : ISendMailService
    {
        private readonly SmtpSettings _smtpSettings;
        public SendMailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task<bool> SendMail(string sender, string recipients, string subject, string body, string? cc, string? cco)
        {
            try
            {
                SmtpClient smtp = new SmtpClient(_smtpSettings.Host)
                {
                    Port = _smtpSettings.Port,
                    EnableSsl = _smtpSettings.EnableSsl,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
                };

                MailAddress from = new MailAddress(sender, "Teste");
                MailAddress to = new MailAddress(recipients, "Teste222");
                MailMessage mail = new MailMessage(from, to)
                {
                    Subject = subject,
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    Body = body,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    IsBodyHtml = true
                };

                // Adiciona o CC se presente
                if (!string.IsNullOrEmpty(cc))
                {
                    mail.CC.Add(cc);
                }

                // Adiciona o CCO se presente
                if (!string.IsNullOrEmpty(cco))
                {
                    mail.Bcc.Add(cco);
                }

                // Envia o email de forma assíncrona
                await smtp.SendMailAsync(mail);

                return true;
            }
            catch (Exception ex)
            {
                // Aqui você pode logar a exceção ou lidar com o erro de outra forma
                Console.WriteLine($"Erro ao enviar o email: {ex.Message}");
                return false;
            }
        }
    }
}
