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
                client.ItemsSource = agencyDbContext.Clients.ToList();
                client.DisplayMemberPath = "Name";
                client.SelectedValuePath = "Id";
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
                    var selling = CreateNewSelling();
                    agencyDbContext.Sellings.Add(selling);
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
                ClientId = (int)client.SelectedValue,
                TourId = (int)tour.SelectedValue,
                Date = DateTime.Now,
                ManagerId = Visitor.Id
            };
            return newSelling;
        }

        private void CheckSpaces()
        {
            if(client.SelectedItem is null)
            {
                throw new Error("Ошибка! Клиент не выбран");
            }

            if (tour.SelectedItem is null)
            {
                throw new Error("Ошибка! Тур не выбран");
            }

            if (Sellings.Exists(selling => selling.ClientId == (client.SelectedItem as Client).Id &&
            selling.TourId == (tour.SelectedItem as Tour).Id))
            {
                throw new Error("Ошибка! Данная операция была проведена ранее");
            }

        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (message.Foreground == Brushes.Red)
            {
                message.Visibility = Visibility.Collapsed;

            }
        }
    }
}
