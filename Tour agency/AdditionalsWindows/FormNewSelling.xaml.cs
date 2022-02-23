using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Tour_agency.Model;

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для FormNewSelling.xaml
    /// </summary>
    public partial class FormNewSelling : Window
    {
        List<Selling> Sellings { get; set; }
        Manager Visitor { get; set; }

        public FormNewSelling(in List<Selling> sellings, Manager visitor)
        {
            InitializeComponent();

            using(var agencyDbContext = new AgencyDbContext())
            {
                cbClient.ItemsSource = agencyDbContext.Clients.ToList();
                cbClient.DisplayMemberPath = "Name";
                cbClient.SelectedValuePath = "Id";
                tour.ItemsSource = agencyDbContext.Tours.ToList();
                tour.DisplayMemberPath = "Name";
                tour.SelectedValuePath = "Id";
            }

            Sellings = sellings;
            Visitor = visitor;
            InstallmentsList.TourIsSelling = true;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            InstallmentsList.TourIsSelling = false;
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckSpaces();
                using(var agencyDbContext = new AgencyDbContext())
                {
                    if (cbClient.Visibility == Visibility.Visible)
                    {
                        var selling = CreateNewSelling();
                        agencyDbContext.Sellings.Add(selling);
                    }
                    else
                    {
                        var newClient = CreateNewClient();
                        agencyDbContext.Clients.Add(newClient);
                        agencyDbContext.SaveChanges();
                        var selling = CreateNewSelling(newClient);
                        agencyDbContext.Sellings.Add(selling);
                    }
                    agencyDbContext.SaveChanges();
                    btnSell.IsEnabled = false;
                    CloseInTime();
                    message.Text = "Тур успешно продан!";
                    message.Foreground = Brushes.White;
                    message.Visibility = Visibility.Visible;
                }
            }
            catch (Error error)
            {
                message.Foreground = Brushes.Red;
                message.Text = error.ToString();
                if(message.Visibility == Visibility.Collapsed)
                {
                    message.Visibility = Visibility.Visible;
                }
            }
        }

        private Client CreateNewClient()
        {
            using(var agencyDbContext = new AgencyDbContext())
            {
                int newIndex = agencyDbContext.Clients.ToList().Last().Id;
                var newClient = new Client()
                {
                    Id = newIndex + 1,
                    Name = tbClient.Text,
                    Phone = phone.Text
                };
                return newClient;
            }
        }

        private void CloseInTime()
        {
            var timer = new DispatcherTimer();
            timer.Tick += (sender, e) => { Close(); };
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            InstallmentsList.TourIsSelling = false;
        }

        private Selling CreateNewSelling()
        {
            Selling newSelling = new Selling()
            {
                ClientId = (int)cbClient.SelectedValue,
                TourId = (int)tour.SelectedValue,
                Date = DateTime.Now,
                ManagerId = Visitor.Id
            };
            return newSelling;
        }

        private Selling CreateNewSelling(Client newClient)
        {
            Selling newSelling = new Selling()
            {
                ClientId = newClient.Id,
                TourId = (int)tour.SelectedValue,
                Date = DateTime.Now,
                ManagerId = Visitor.Id
            };
            return newSelling;
        }

        private void CheckSpaces()
        {
            if(cbClient.Visibility == Visibility.Visible)
            {
                if (cbClient.SelectedItem is null)
                {
                    throw new Error("Ошибка! Клиент не выбран");
                }
                if (Sellings.Exists(selling => selling.ClientId == (cbClient.SelectedItem as Client).Id &&
                                    selling.TourId == (tour.SelectedItem as Tour).Id))
                {
                    throw new Error("Ошибка! Данная операция была проведена ранее");
                }
            }
            else
            {
                if(tbClient.Text.Length == 0)
                {
                    throw new Error("Ошибка! Введите ФИО клиента");
                }

                if(phone.Text.Length < 17)
                {
                    throw new Error("Ошибка! Номер телефона введён не полностью");
                }
            }

            if (tour.SelectedItem is null)
            {
                throw new Error("Ошибка! Тур не выбран");
            }

        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (message.Foreground == Brushes.Red)
            {
                message.Visibility = Visibility.Collapsed;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            labelPhone.Visibility = Visibility.Visible;
            phone.Visibility = Visibility.Visible;
            tbClient.Visibility = Visibility.Visible;
            cbClient.Visibility = Visibility.Collapsed;
            labelPanel.VerticalAlignment = VerticalAlignment.Bottom;
            boxesPanel.VerticalAlignment = VerticalAlignment.Bottom;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            labelPhone.Visibility = Visibility.Collapsed;
            phone.Visibility = Visibility.Collapsed;
            tbClient.Visibility = Visibility.Collapsed;
            cbClient.Visibility = Visibility.Visible;
            labelPanel.VerticalAlignment = VerticalAlignment.Center;
            boxesPanel.VerticalAlignment = VerticalAlignment.Center;
        }

        private void phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int num;
            if(!Int32.TryParse(e.Text, out num)){
                e.Handled = true;
            }
        }

        private void phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (phone.Text.Length)
            {
                case 0: { phone.Text += '+'; break; }
                case 1: { phone.Text += '7'; break; }
                case 2: { phone.Text += '('; break; }
                case 6: { phone.Text += ')'; break; }
                case 7: { phone.Text += ' '; break; }
                case 11: { phone.Text += ' '; break; }
                case 14: { phone.Text += '-'; break; }
                default: break;
            }
            phone.SelectionStart = phone.Text.Length;
        }

        private void phone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
