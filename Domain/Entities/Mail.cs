namespace Domain.Entities
{
    /// <summary>
    /// Письмо.
    /// </summary>
    public class Mail
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Отправитель.
        /// </summary>
        public Guid Sender { get; set; }

        /// <summary>
        /// Получатель.
        /// </summary>
        public Guid Destination { get; set; }

        /// <summary>
        /// Тело письма.
        /// </summary>
        public string Body { get; set; }
    }
}
