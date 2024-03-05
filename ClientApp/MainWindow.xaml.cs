using Domain.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClientApp
{
    /// <summary>
    /// Главное окно.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Клиент.
        /// </summary>
        private static readonly HttpClient client = new();

        /// <summary>
        /// Url сервиса.
        /// </summary>
        private string serviceUrl;

        /// <summary>
        /// Ввошедший пользователь.
        /// </summary>
        private readonly Employee _currentUser;

        /// <summary>
        /// Список сотрудников (за исключением ввошедшего пользователя).
        /// </summary>
        private List<Employee> _employees;

        /// <summary>
        /// Инициализация окна.
        /// </summary>
        /// <param name="employee">Ввошедший пользователь.</param>
        public MainWindow(Employee employee)
        {
            InitializeComponent();

            _currentUser = employee;

            Initialization();
        }

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <returns>Начальная настройка.</returns>
        public async Task Initialization()
        {
            var sericeUrlTextBlock = Application.Current.FindResource("ServiceUrl") as TextBlock;

            serviceUrl = sericeUrlTextBlock.Text;

            var response = await client.GetAsync($"{serviceUrl}/api/Employee/GetAllEmployees");

            if (response.IsSuccessStatusCode)
            {
                var employeesJson = await response.Content.ReadAsStringAsync();

                _employees = JsonConvert.DeserializeObject<List<Employee>>(employeesJson);
            }

            if (_employees.Count > 0)
            {
                _employees.RemoveAll(e => e.Email == _currentUser.Email);

                for(int i = 0; i < _employees.Count; i++)
                {
                    Destination.Items.Add(_employees[i].Email);
                }
            }
        }

        /// <summary>
        /// Отправка сообщения. 
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Маршрутизируемые события.</param>
        private async void SendMessageButton_click(object sender, RoutedEventArgs e)
        {
            RegisterMail.Text = string.Empty;
            SendMail.Text = string.Empty;

            if (string.IsNullOrEmpty(Destination.Text))
            {
                RegisterMail.Foreground = new SolidColorBrush(Colors.Red);
                RegisterMail.Text = "Выберите получателя сообщения";
                return;
            }

            if (string.IsNullOrEmpty(Title.Text))
            {
                RegisterMail.Foreground = new SolidColorBrush(Colors.Red);
                RegisterMail.Text = "Введите заголовок сообщения";
                return;
            }

            if (string.IsNullOrEmpty(Body.Text))
            {
                RegisterMail.Foreground = new SolidColorBrush(Colors.Red);
                RegisterMail.Text = "Введите текст сообщения";
                return;
            }

            try
            {
                await RegisterMessage();
            }
            catch
            {
                RegisterMail.Foreground = new SolidColorBrush(Colors.Red);
                RegisterMail.Text = "Ошибка при регистрации письма";
            }

            try
            {
                await SendMessage();
            }
            catch
            {
                SendMail.Foreground = new SolidColorBrush(Colors.Red);
                SendMail.Text = "Ошибка при отправки письма";
            }

        }

        /// <summary>
        /// Регистрация сообщения.
        /// </summary>
        /// <returns>Результат.</returns>
        private async Task RegisterMessage()
        {
            try
            {
                var content = GetMailStringContent();

                var response = await client.PostAsync($"{serviceUrl}/api/Mail/RegisterMessage", await content);

                if (response.IsSuccessStatusCode)
                {
                    RegisterMail.Foreground = new SolidColorBrush(Colors.Green);
                    RegisterMail.Text = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    RegisterMail.Foreground = new SolidColorBrush(Colors.Red);
                    RegisterMail.Text = "Письмо не зарегистрировано в системе";
                }
            }
            catch 
            {
                RegisterMail.Foreground = new SolidColorBrush(Colors.Red);
                RegisterMail.Text = "Сервер не отвечает на запрос";
            }
        }

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <returns>Результат.</returns>
        private async Task SendMessage()
        {
            try
            {
                var content = GetMailStringContent();

                var response = await client.PostAsync($"{serviceUrl}/api/Mail/SendMessage", await content);

                if (response.IsSuccessStatusCode)
                {
                    SendMail.Foreground = new SolidColorBrush(Colors.Green);
                    SendMail.Text = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    SendMail.Foreground = new SolidColorBrush(Colors.Red);
                    SendMail.Text = await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                SendMail.Foreground = new SolidColorBrush(Colors.Red);
                SendMail.Text = "Сервер не отвечает на запрос";
            }
        }

        /// <summary>
        /// Получение контента письма.
        /// </summary>
        /// <returns>Строка контента.</returns>
        private async Task<StringContent> GetMailStringContent()
        {
            var destinationUserResponce = await client.GetAsync($"{serviceUrl}/api/Employee/GetEmployeeByEmail?email={Destination.Text}");

            var destinationUserJson = await destinationUserResponce.Content.ReadAsStringAsync();

            var destinationUser = JsonConvert.DeserializeObject<Employee>(destinationUserJson);

            var json = JsonConvert.SerializeObject(new Mail
            {
                Title = this.Title.Text,
                Sender = _currentUser.Id,
                Destination = destinationUser.Id,
                Body = Body.Text
            });

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
