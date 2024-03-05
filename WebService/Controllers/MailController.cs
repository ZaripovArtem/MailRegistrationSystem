using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.Services.Interfaces;

namespace WebService.Controllers
{
    /// <summary>
    /// Контроллер писем.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        /// <summary>
        /// Интерфейс сервиса отправки сообщений.
        /// </summary>
        private readonly IMailService _mailService;

        /// <summary>
        /// Контекст данных.
        /// </summary>
        private readonly ApplicationContext _context;

        /// <summary>
        /// Инициализация контроллера.
        /// </summary>
        /// <param name="mailService">Сервис отправки сообщений.</param>
        /// <param name="context">Контекст данных.</param>
        public MailController(IMailService mailService, ApplicationContext context)
        {
            _mailService = mailService;
            _context = context;
        }

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="mail">Сообщение.</param>
        /// <returns>Результат отправки сооющения.</returns>
        [HttpPost]
        public async Task<IActionResult> SendMessage(Mail mail)
        {
            if (mail.Sender == mail.Destination)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Отправитель и получатель должен быть разным.");
            }

            var sender = await _context.Employees.Where(e => e.Id == mail.Sender).FirstOrDefaultAsync();

            if (sender == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Отправитель не найден.");
            }

            var destination = await _context.Employees.Where(e => e.Id == mail.Destination).FirstOrDefaultAsync();

            if (destination == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Получатель не найден.");
            }

            var result = await _mailService.SendAsync(mail);

            return result
                ? StatusCode(StatusCodes.Status200OK, "Письмо было отправлено.")
                : StatusCode(StatusCodes.Status400BadRequest, "Ошибка с отправкой письма.");
        }

        /// <summary>
        /// Регистрация сообщения.
        /// </summary>
        /// <param name="mail">Сообщение.</param>
        /// <returns>Результат регистрации сообщения.</returns>
        [HttpPost]
        public async Task<IActionResult> RegisterMessage(Mail mail)
        {
            try
            {
                var registerMail = new MailRegistration();

                registerMail.Mail = mail;

                registerMail.Date = DateTimeOffset.Now;

                await _context.MailRegistrations.AddAsync(registerMail);

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, "Письмо зарегистрировано.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Ошибка с регистрацией письма.");
            }
        }
    }
}
