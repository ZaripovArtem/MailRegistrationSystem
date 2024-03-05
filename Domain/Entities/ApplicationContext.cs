using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    /// <summary>
    /// Контекст данных.
    /// </summary>
    public class ApplicationContext : DbContext
    {
        /// <summary>
        /// Сотрудники.
        /// </summary>
        public DbSet<Employee> Employees { get; set; } = null!;

        /// <summary>
        /// Регистрация писем.
        /// </summary>
        public DbSet<MailRegistration> MailRegistrations { get; set; } = null!;

        /// <summary>
        /// Письма.
        /// </summary>
        public DbSet<Mail> Mails { get; set; } = null!;

        /// <summary>
        /// Контекст данных.
        /// </summary>
        /// <param name="options">Параметры.</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Заполнение начальными 
        /// </summary>
        /// <param name="modelBuilder">Билдер.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee 
                {
                    Id = new Guid("401677E5-9643-4A81-9D23-794CDB22EA69"),
                    FullName = "ТестИмя1",
                    Post = "Тест",
                    Email = "test@gmail.com",
                    // Пароль Qwerty
                    Password = "e48a0d76a5c440915d35fb46691cde132de2cd06575fdb72b38d8b0679fb3012"
                },
                new Employee
                {
                    Id = new Guid("7747EB42-1E70-48CC-A40C-9ECA44148581"),
                    FullName = "ТестИмя2",
                    Post = "Тест",
                    Email = "artemzaripov2002@gmail.com",
                    // Пароль Test
                    Password = "73fed31243c47293f77b99bfe34f1d792cd2913ec6e244721e248f9941524db4",
                }
            );
        }
    }
}
