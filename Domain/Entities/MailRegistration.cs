namespace Domain.Entities
{
    /// <summary>
    /// Регистрация письма.
    /// </summary>
    public class MailRegistration
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Письмо.
        /// </summary>
        public Mail Mail { get; set; }

        /// <summary>
        /// Дата отправки.
        /// </summary>
        public DateTimeOffset Date { get; set; }
    }
}
