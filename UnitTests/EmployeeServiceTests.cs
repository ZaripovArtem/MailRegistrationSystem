using WebService.Services;

namespace UnitTests
{
    /// <summary>
    /// Тестирование сервиса сотрудников.
    /// </summary>
    [TestClass]
    public class EmployeeServiceTests
    {
        /// <summary>
        /// Тестирование метода генерации хэша пароля.
        /// </summary>
        [TestMethod]
        public void GenerateSaltedHash_PasswordAndSalt_ReturnHash()
        {
            var employeeHandler = new EmployeeService();

            var salt = "c2F5bWluZw==";

            var result = employeeHandler.GenerateSaltedHash("Qweasd", salt);

            Assert.AreEqual("a9d9b261e9b48ff19beaf0cfdba9806c64f04a12cad23f7b3ab6328e14b8d1c5", result);
        }
    }
}