namespace WebService.Services.Interfaces
{
    /// <summary>
    /// Обработчик работника.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Генерация хэша пароля.
        /// </summary>
        /// <param name="password">Пароль.</param>
        /// <param name="salt">Соль.</param>
        /// <returns>Хэш пароля.</returns>
        public string GenerateSaltedHash(string password, string salt);
    }
}
