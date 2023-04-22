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
using VideoRenderHermes.ADOApp;

namespace VideoRenderHermes.PagesApp
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        private int authCount;
        public AuthorizationPage()
        {
            InitializeComponent();
            if(Properties.Settings.Default.Login != null)
            {
                tbLogin.Text = Properties.Settings.Default.Login;   
            }
        }

        private void Authorization(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text != "" && tbPassword.Text != "")
            {
                if(authCount == 3)
                {
                    Properties.Settings.Default.BlockTime = DateTime.Now.AddMinutes(1);
                    Properties.Settings.Default.Save();
                    authCount = 0;
                }

                if(Properties.Settings.Default.BlockTime > DateTime.Now) 
                {
                    MessageBox.Show("Слишком много неверных попыток! Попробуйте снова через 1 минуту!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                User user = App.Connection.User.FirstOrDefault(x => x.Login == tbLogin.Text);

                if(user == null)
                {
                    MessageBox.Show("Пользователя с таким логином не существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if(user.Password != tbPassword.Text) 
                {
                    MessageBox.Show("Неверный пароль!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    authCount++;
                    return;
                } 

                if ((bool)cbRemember.IsChecked) 
                {
                    Properties.Settings.Default.Login = tbLogin.Text;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.Login = null;
                    Properties.Settings.Default.Save();
                }

                MessageBox.Show("Авторизация прошла успешно!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new MainPage());
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoToRegPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
