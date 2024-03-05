using Domain.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ClientApp
{
    /// <summary>
    /// Окно авторизации.
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// Клиент.
        /// </summary>
        private static readonly HttpClient client = new();

        /// <summary>
        /// Инициализация окна.
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Маршрутизируемые события.</param>
        private async void AuthorizeButton_click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = string.Empty;

            if (string.IsNullOrEmpty(Email.Text))
            {
                ErrorMessage.Text = "Введите Email";
                return;
            }

            if (string.IsNullOrEmpty(Password.Text))
            {
                ErrorMessage.Text = "Введите пароль.";
                return;
            }

            var sericeUrlTextBlock = Application.Current.FindResource("ServiceUrl") as TextBlock;
            string serviceUrl = sericeUrlTextBlock.Text;

            var json = JsonConvert.SerializeObject(new Domain.Entities.Authorization 
            { 
                Login = Email.Text, 
                Password = Password.Text 
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync($"{serviceUrl}/api/Employee/Authorization", content);

                if (response.IsSuccessStatusCode)
                {
                    var userJson = await response.Content.ReadAsStringAsync();

                    var user = JsonConvert.DeserializeObject<Employee>(userJson);

                    var mainWindow = new MainWindow(user);

                    this.Close();

                    mainWindow.Show();
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ErrorMessage.Text = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        ErrorMessage.Text = "Что-то пошло не так.";
                    }
                }
            }
            catch
            {
                ErrorMessage.Text = "Ошибка с доступом к сервису.";
            }
        }
    }
}