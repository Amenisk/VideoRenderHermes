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
using VideoRenderHermes.PagesApp;

namespace VideoRenderHermes
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AuthorizationPage());
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (MainFrame.NavigationService.CanGoBack)
            {
                switch ((MainFrame.Content as Page).Title)
                {
                    case "RegistrationPage":
                        MainFrame.Navigate(new AuthorizationPage());
                        break;

                    case "MainPage":
                        MainFrame.Navigate(new AuthorizationPage());
                        break;

                    case "AddClientPage":
                        MainFrame.Navigate(new MainPage());
                        break;

                    case "VisitsPage":
                        MainFrame.Navigate(new MainPage());
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
