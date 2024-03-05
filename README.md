# Прототип системы для регистрации входящих писем, адресованных сотрудникам компании.

# Техническое задание - [ссылка](https://1drv.ms/w/c/b734f2b16a3a311b/EcaBk7SKHpxKlkIhfVHquckBHJTnDEWAPRCNJ7_Q676VJw?e=MdPOGp)

# Окна клиентской программы:
![image](https://github.com/ZaripovArtem/MailRegistrationSystem/assets/78857901/10a2c9e4-5f32-4605-bc26-e6ba3c8a2641)
![image](https://github.com/ZaripovArtem/MailRegistrationSystem/assets/78857901/66c6f722-ba3e-47bd-be2b-3dd495da1d21)

# Первоначальная настройка
1 - Склонировать репозиторий.

2 - Настроить сервис отправки сообщений на электронную почту (по желанию, без этого регистрация сообщений будет также функционировать). Для этого перейдите в проект WebService в файл appsettings.json и введите следующие поля:

![image](https://github.com/ZaripovArtem/MailRegistrationSystem/assets/78857901/05d2910e-a5b3-47d5-9cb2-2330ca08f461)

При необходимости, измените значение Port.

3 - При необходимости, изменить начальные значения пользователей в базе данных. Для этого перейдите в проект Domain.Entities в класс ApplicationContext.cs и в методе OnModelCreating поменяйте данные, которые будут записаны при 1 запуске веб-сервиса.

4 - Запустите проект WebService.

5 - Внутри проекта ClientApp в файле App.xaml проверить Url сервиса (8 строчка).

![image](https://github.com/ZaripovArtem/MailRegistrationSystem/assets/78857901/6694b287-494c-44b0-aed1-41ff2935f83c)

6 - После всех действий, можно запускать клиентское приложение (проект ClientApp).

# Тестовые данные

Начальные значения тестовых пользователей описаны в проекте Domain.Entities в файле ApplicationContext

Для проверки системы предусмотрено 2 пользователя:

Логин - test@gmail.com

Пароль - Qwerty

Логин - artemzaripov2002@gmail.com

Пароль - Test

Для проверки работоспособности метода отправки писем, необходимо изменить в файле ApplicationContext метод OnModelCreating, изменив Email на ваш личный (см. 3 пункт первоначальной настройки). После чего зайти с пользователя test@gmail.com и отправить самому себе письмо, при условии, что настроен сервис отправки сообщений (см. 2 пункт первоначальной настройки).
