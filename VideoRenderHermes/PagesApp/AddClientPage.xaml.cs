using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddClientPage.xaml
    /// </summary>
    public partial class AddClientPage : Page
    {
        private byte[] _photo;
        private Client _client;
        List<Tag> tagsList = new List<Tag>();
        List<Tag> tagsComboList = new List<Tag>();

        public AddClientPage()
        {
            InitializeComponent();
            tagsComboList = App.Connection.Tag.ToList();
            cmbTags.ItemsSource = tagsComboList;
            tbId.Text += "ID: " + (App.Connection.Client.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Id != 0).Id + 1).ToString();
        }

        public AddClientPage(Client client)
        {
            InitializeComponent();
            _client = client;
            this.DataContext = _client;
            if(client.GenderID == 1)
            {
                rbM.IsChecked = true;
            }
            else
            {
                rbW.IsChecked = true;
            }
            if(client.Photo != null)
            {
                iPhoto.Source = BytesToImage(client.Photo);
            }

            foreach (var ct in client.ClientTag)
            {
                tagsList.Add(ct.Tag);
            }
            lvTags.ItemsSource = tagsList;

            tagsComboList = App.Connection.Tag.ToList();
            tagsComboList = tagsComboList.Where(x => !tagsList.Contains(x)).ToList();
            cmbTags.ItemsSource = tagsComboList;
        }

        private void SelectPhoto(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() != null)
                {
                    _photo = File.ReadAllBytes(dialog.FileName);
                    MessageBox.Show("Фото загружено!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    iPhoto.Source = BytesToImage(_photo);
                    if(_client != null)
                    {
                        _client.Photo = _photo;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Только фото!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private BitmapImage BytesToImage(byte[] bytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memoryStream;
                image.EndInit();
                return image;
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if(tbFirstName.Text != null && tbLastName.Text != null && tbLastName.Text != null 
                && tbEmail.Text != null && tbPhoneNumber.Text != null && dpBirthDate.SelectedDate != null 
                && iPhoto.Source != null && ((bool) rbM.IsChecked || (bool) rbW.IsChecked))
            {
                if(!CheckNamesAndPatronymic())
                {
                    return;
                }

                if(!CheckEmail())
                {
                    MessageBox.Show("Некорректный Email!", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!CheckPnoneNumber())
                {
                    MessageBox.Show("Некорректный номер телефона!", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int genderId;
                if ((bool)rbM.IsChecked)
                {
                    genderId = 1;
                }
                else
                {
                    genderId = 2;
                }

                if(_client == null)
                {
                    var client = new Client()
                    {
                        Id = Convert.ToInt32(tbId.Text.Substring(4)),
                        Firstname = tbFirstName.Text,
                        Lastname = tbLastName.Text,
                        Patronymic = tbPatronymic.Text,
                        Email = tbEmail.Text,
                        PhoneNumber = tbPhoneNumber.Text,
                        BirthDate = (DateTime)dpBirthDate.SelectedDate,
                        GenderID = genderId,
                        Photo = _photo,
                        AddedDate = DateTime.Now,
                    };

                    foreach(var tag in tagsList)
                    {
                        var clientTag = new ClientTag()
                        {
                            ClientId = client.Id,
                            TagId = tag.Id,
                        };

                        App.Connection.ClientTag.Add(clientTag);
                    }

                    App.Connection.Client.Add(client);
                    MessageBox.Show("Добавление клиента прошло успешно!", "Успешно!",
                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Редактирование клиента прошло успешно!", "Успешно!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                App.Connection.SaveChanges();
                NavigationService.Navigate(new MainPage());
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckNamesAndPatronymic()
        {
            if(tbFirstName.Text.Length > 50) 
            {
                MessageBox.Show("Длина фамилии превышает 50 символов!", "Ошибка!", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (tbLastName.Text.Length > 50)
            {
                MessageBox.Show("Длина имени превышает 50 символов!", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (tbPatronymic.Text.Length > 50)
            {
                MessageBox.Show("Длина отчества превышает 50 символов!", "Ошибка!", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            foreach(var c in tbFirstName.Text)
            {
                if(!((c >= 'А' && c <= 'я') || c == '-' || c == ' '))
                {
                    MessageBox.Show("Фамилия может включать в себя только буквы, дефиз и пробел!", "Ошибка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            foreach (var c in tbLastName.Text)
            {
                if (!((c >= 'А' && c <= 'я') || c == '-' || c == ' '))
                {
                    MessageBox.Show("Имя может включать в себя только буквы, дефиз и пробел!", "Ошибка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            foreach (var c in tbPatronymic.Text)
            {
                if (!((c >= 'А' && c <= 'я') || c == '-' || c == ' '))
                {
                    MessageBox.Show("Отчество может включать в себя только буквы, дефиз и пробел!", "Ошибка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }


            return true;
        }

        private bool CheckEmail()
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(tbEmail.Text, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        private bool CheckPnoneNumber()
        {
            string pattern = "^((8|\\+7)[\\- ]?)?(\\(?\\d{3}\\)?[\\- ]?)?[\\d\\- ]{7,10}$";
            Match isMatch = Regex.Match(tbPhoneNumber.Text, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        private void AddTag(object sender, RoutedEventArgs e)
        {
            if(cmbTags.SelectedItem != null) 
            {
                if(_client != null)
                {
                    var clientTag = new ClientTag()
                    {
                        TagId = ((Tag)cmbTags.SelectedItem).Id,
                        ClientId = _client.Id
                    };

                    App.Connection.ClientTag.Add(clientTag);
                    App.Connection.SaveChanges();
                }

                tagsList.Add((Tag)cmbTags.SelectedItem);
                tagsComboList.Remove((Tag)cmbTags.SelectedItem);
                cmbTags.SelectedIndex = -1;
                lvTags.ItemsSource = null;
                cmbTags.ItemsSource = null;
                lvTags.ItemsSource = tagsList;
                cmbTags.ItemsSource = tagsComboList;              
            }
            else
            {
                MessageBox.Show("Сначала выберите тег из выпадающего списка!", "Ошибка!",
                       MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTag(object sender, RoutedEventArgs e)
        {
            if (lvTags.SelectedItem != null)
            {
                if (_client != null)
                {
                    var clientTag = App.Connection.ClientTag.FirstOrDefault
                        (x => x.TagId == ((Tag)lvTags.SelectedItem).Id && x.ClientId == _client.Id );

                    App.Connection.ClientTag.Remove(clientTag);
                    App.Connection.SaveChanges();
                }

                tagsComboList.Add((Tag)lvTags.SelectedItem);
                tagsList.Remove((Tag)lvTags.SelectedItem);
                lvTags.SelectedIndex = -1;
                lvTags.ItemsSource = null;
                cmbTags.ItemsSource = null;
                lvTags.ItemsSource = tagsList;
                cmbTags.ItemsSource = tagsComboList;
            }
            else
            {
                MessageBox.Show("Сначала выберите тег из списка!", "Ошибка!",
                       MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
