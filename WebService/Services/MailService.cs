using Domain.Entities;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using WebService.Services.Interfaces;

namespace WebService.Services
{
    public class MailService : IMailService
    {
        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Настройки для почты.
        /// </summary>
        private readonly MailSettings _settings;

        /// <summary>
        /// Контекст данных.
        /// </summary>
        private readonly ApplicationContext _context;


        /// <summary>
        /// Инициализация сервиса.
        /// </summary>
        /// <param name="settings">Настройки для почты.</param>
        /// <param name="context">Контекст данных.</param>
        public MailService(IOptions<MailSettings> settings, ApplicationContext context)
        {
            _settings = settings.Value;
            _context = context;
        }

        /// <summary>
        /// Отправка сообщений.
        /// </summary>
        /// <param name="emailBody">Тело сообщения.</param>
        /// <returns>True - успешно, false - нет.</returns>
        public async Task<bool> SendAsync(Mail emailBody)
        {
            try
            {
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
                mail.Sender = new MailboxAddress(_settings.DisplayName, _settings.From);
                mail.To.Add(MailboxAddress.Parse(_context.Employees
                    .Where(e => e.Id == emailBody.Destination)
                    .Select(e => e.Email)
                    .FirstOrDefault()));

                var body = new BodyBuilder();
                mail.Subject = emailBody.Title;
                body.HtmlBody = emailBody.Body;
                mail.Body = body.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    try
                    {
                        await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(_settings.UserName, _settings.Password);
                        await client.SendAsync(mail);
                    }
                    catch (Exception e)
                    {
                        ErrorMessage = e.ToString();
                        throw;
                    }
                    finally
                    {
                        await client.DisconnectAsync(true);
                        client.Dispose();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                ErrorMessage = e.ToString();

                return false;
            }
        }
    }
}
