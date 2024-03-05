using Domain.Entities;

namespace WebService.Services.Interfaces
{
    public interface IMailService
    {
        /// <summary>
        /// Сообщение об ошибки.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="mail">Тело сообщения.</param>
        /// <returns>True - успешно, false - нет.</returns>
        public Task<bool> SendAsync(Mail mail);
    }
}
