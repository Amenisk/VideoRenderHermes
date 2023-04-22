using VideoRenderHermes.ADOApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VideoRenderHermes.PagesApp
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void Registration(object sender, RoutedEventArgs e)
        {
            if(tbLogin.Text != "" && tbPassword.Text != "")
            {
                if(!CheckPassword(tbPassword.Text))
                {
                    return;
                }

                var user = new User
                { 
                    Login = tbLogin.Text,
                    Password = tbPassword.Text,
                };

                var checkUser = App.Connection.User.FirstOrDefault(x => x.Login == tbLogin.Text);

                if(checkUser == null)
                {
                    App.Connection.User.Add(user);
                    App.Connection.SaveChanges();
                    MessageBox.Show("Регистрация прошла успешно!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new AuthorizationPage());
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckPassword(string password) //проверка пароля на корректность
        {
            bool check = false;
            List<char> checkSymbols = new List<char>()
            {
                '!', '@', '#', '$', '%', '^'
            }; 

            if(password.Length < 6)
            {
                MessageBox.Show("Пароль должен быть больше 6 символов!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            foreach(char c in password)
            {
                if(c >= 'A' && c <= 'Z') 
                {
                    check = true;
                    break;
                }
            }

            if(!check)
            {
                MessageBox.Show("Пароль должен содержать минимум 1 прописную букву!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            check = false;

            foreach (char c in password)
            {
                if (c >= '0' && c <= '9')
                {
                    check = true;
                    break;
                }
            }

            if (!check)
            {
                MessageBox.Show("Пароль должен содержать минимум 1 цифру!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            check = false;

            foreach (char c in password)
            {
                foreach(char ch in checkSymbols)
                {
                    if(c == ch)
                    {
                        check = true;
                        break;
                    }
                }
            }

            if (!check)
            {
                MessageBox.Show("Пароль должен содержать минимум 1 специальный символ (! @ # $ % ^)!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void GoToAuthPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
