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
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoRenderHermes.ADOApp;
using VideoRenderHermes.Classes;

namespace VideoRenderHermes.PagesApp
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private List<ClientInformation> clientsInformation = new List<ClientInformation>();
        private List<ClientInformation> sortClientInf = new List<ClientInformation>();
        private List<ClientInformation> pageSortInf = new List<ClientInformation>();
        private int pageNumber;
        private int countRecords;

        public MainPage()
        {
            InitializeComponent();

            foreach (var client in App.Connection.Client.ToList())
            {
                clientsInformation.Add(new ClientInformation(client));
            }
            sortClientInf = clientsInformation;
            lvClients.ItemsSource = sortClientInf;
            cmbCountRecords.SelectedIndex = 0;
            cmbFilter.SelectedIndex = 0;
            cmbSort.SelectedIndex = 0;
            pageNumber = 1;
            countRecords = App.Connection.Client.Count();
            tbCountRecords.Text = $"{clientsInformation.Count} из {clientsInformation.Count}";
        }

        private void Sort() //выполнение поиска, сорировки и фильтрации
        {
            sortClientInf = clientsInformation;

            if (tbSearch.Text != "")
            {
                sortClientInf = clientsInformation.Where(x => x.Firstname.Contains(tbSearch.Text) || x.Lastname.Contains(tbSearch.Text)
            || x.Patronymic.Contains(tbSearch.Text) || x.PhoneNumber.Contains(tbSearch.Text) || x.Email.Contains(tbSearch.Text)).ToList();
            }   
            
            switch(cmbFilter.SelectedIndex)
            {
                case 0:
                    break;

                case 1:
                    sortClientInf = sortClientInf.Where(x => x.Gender == "Мужской").ToList();
                    break;

                case 2:
                    sortClientInf = sortClientInf.Where(x => x.Gender == "Женский").ToList();
                    break;

                default:
                    break;
            }

            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    break;

                case 1:
                    sortClientInf = sortClientInf.OrderBy(x => x.Firstname).ToList();
                    break;

                case 2:
                    sortClientInf = sortClientInf.OrderByDescending(x => x.LastDate).ToList();   
                    break;

                case 3:
                    sortClientInf = sortClientInf.OrderByDescending(x => x.VisitCount).ToList();
                    break;

                default:
                    break;
            }
            
            if(cbBirthday.IsChecked == true)
            {
                sortClientInf = sortClientInf.Where(x => x.BirthDate.Month == DateTime.Now.Month).ToList();
            }

            pageSortInf = sortClientInf.Skip((pageNumber - 1) * countRecords).Take(countRecords).ToList();

            tbCountRecords.Text = $"{pageSortInf.Count} из {clientsInformation.Count()}";
            lvClients.ItemsSource = pageSortInf;
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            Sort();
        }

        private void Filter(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        private void Sorting(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        private void cbBirthday_Checked(object sender, RoutedEventArgs e)
        {
            Sort();
        }

        private void cmbCountrecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangePageRecordsCount();
            Sort();
        }

        private void ChangePageRecordsCount()
        {
            switch (cmbCountRecords.SelectedIndex)
            {
                case 0:
                    countRecords = App.Connection.Client.Count();
                    break;

                case 1:
                    countRecords = 10;
                    break;

                case 2:
                    countRecords = 50;
                    break;

                case 3:
                    countRecords = 200;
                    break;

                default:
                    break;
            }

            pageNumber = 1;
            tbPageCount.Text = pageNumber.ToString();
        }

        private void BackPage(object sender, RoutedEventArgs e)
        {
            if(pageNumber > 1)
            {
                pageNumber--;
                tbPageCount.Text = pageNumber.ToString();
                Sort();
            }
        }

        private void NextPage(object sender, RoutedEventArgs e)
        {
            if(pageNumber * countRecords < sortClientInf.Count)
            {
                pageNumber++;
                tbPageCount.Text = pageNumber.ToString();
                Sort();
            }
        }

        private void AddClient(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddClientPage());
        }

        private void ChangeClient(object sender, RoutedEventArgs e)
        {
            var tag = (int)((Button)sender).Tag;
            NavigationService.Navigate(new AddClientPage(App.Connection.Client.FirstOrDefault(x => x.Id == tag)));
        }

        private void DeleteClient(object sender, RoutedEventArgs e)
        {
            var tag = (int)((Button)sender).Tag;
            var client = App.Connection.Client.FirstOrDefault(x => x.Id == tag);
            if (App.Connection.Visit.FirstOrDefault(x => x.ClientId == client.Id) == null)
            {
                foreach(var clInf in sortClientInf)
                {
                    if(clInf.Id == tag)
                    {
                        clientsInformation.Remove(clInf);
                        break;
                    }
                }
                App.Connection.Client.Remove(App.Connection.Client.FirstOrDefault(x => x.Id == tag));
                App.Connection.SaveChanges();
                MessageBox.Show("Удаление прошло успешно!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                Sort();
            }
            else
            {
                MessageBox.Show("Удаление невозможно, так как" +
                    " в базе хранятся данные о визитах этого клиента!", "Ошибка!", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        private void GoToVisits(object sender, RoutedEventArgs e)
        {
            var tag = (int)((Button)sender).Tag;
            NavigationService.Navigate(new VisitsPage(App.Connection.Client.FirstOrDefault(x => x.Id == tag)));
        }
    }
}
