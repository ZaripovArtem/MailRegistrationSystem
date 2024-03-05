namespace Domain.Entities
{
    /// <summary>
    /// Сотрудник.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Фио.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Должность.
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// Электронная почта.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }
    }
}
