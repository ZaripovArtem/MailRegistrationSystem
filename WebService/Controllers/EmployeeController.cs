using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.Services.Interfaces;

namespace WebService.Controllers
{
    /// <summary>
    /// Контроллер сотрудников.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        /// <summary>
        /// Конфигурация.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Контекст данных.
        /// </summary>
        private readonly ApplicationContext _context;

        /// <summary>
        /// Сервис сотрудников.
        /// </summary>
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="context">Контекст данных.</param>
        /// <param name="employeeService">Сервис сотрудников.</param>
        public EmployeeController(IConfiguration configuration, ApplicationContext context, IEmployeeService employeeService)
        {
            _configuration = configuration;
            _context = context;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Получение всех сотрудников.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IQueryable<Employee>> GetAllEmployees()
        {
            return await Task.FromResult(_context.Employees);
        }

        /// <summary>
        /// Получение сотрудника по email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>Сотрудник.</returns>
        [HttpGet]
        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            return await _context.Employees.Where(e => e.Email == email).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <param name="authorization">Данные авторизации.</param>
        /// <returns>Статус авторизации.</returns>
        [HttpPost]
        public async Task<IActionResult> Authorization(Authorization authorization)
        {
            var passwordHash = _employeeService.GenerateSaltedHash(authorization.Password, _configuration["PasswordSettings:Hash"]);

            var employee = await _context.Employees.Where(e => e.Email == authorization.Login).FirstOrDefaultAsync();

            if (employee == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Пользователь не найден.");
            }

            return employee.Password == passwordHash
                ? StatusCode(StatusCodes.Status200OK, employee)
                : StatusCode(StatusCodes.Status400BadRequest, "Неправильно введен пароль.");
        }
    }
}
