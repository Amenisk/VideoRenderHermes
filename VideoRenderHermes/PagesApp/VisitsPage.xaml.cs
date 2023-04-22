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
    /// Логика взаимодействия для VisitsPage.xaml
    /// </summary>
    public partial class VisitsPage : Page
    {
        public VisitsPage(Client client)
        {
            InitializeComponent();
            tbName.Text = $"{client.Firstname} {client.Lastname} {client.Patronymic}";
            lvVisits.ItemsSource = App.Connection.Visit.Where(x => x.ClientId == client.Id).ToList();
        }
    }
}
