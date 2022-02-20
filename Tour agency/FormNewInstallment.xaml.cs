using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using Tour_agency.Model;

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для FormNewInstallment.xaml
    /// </summary>
    public partial class FormNewInstallment : Window
    {

        List<Selling> Sellings { get; set; }
        List<Installment> Installments { get; set; }

        public FormNewInstallment(List<Selling> sellings, List<Installment> installments)
        {
            InitializeComponent();

            using (var agencyDbContext = new AgencyDbContext())
            {
                client.ItemsSource = agencyDbContext.Clients.ToList();
                client.DisplayMemberPath = "Name";
                client.SelectedValuePath = "Id";
                tour.ItemsSource = agencyDbContext.Tours.ToList();
                tour.DisplayMemberPath = "Name";
                tour.SelectedValuePath = "Id";
            }

            Sellings = sellings;
            Installments = installments;

        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void period_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int periodInt;
            if(!(Int32.TryParse(e.Text, out periodInt) && (period.Text.Length == 0 && periodInt != 0 ||
                period.Text.Length > 0)))
            {
                e.Handled = true;
            }
        }

        private void period_PreviewKeyDown(object sender, KeyEventArgs e)
        {
           if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckSpaces();
                using (var agencyDbContext = new AgencyDbContext())
                {
                    var selling = CreateNewInstallment();
                    agencyDbContext.Installments.Add(selling);
                    agencyDbContext.SaveChanges();
                    btnSell.IsEnabled = false;
                    CloseInTime();
                    message.Text = "Рассрочка успешно добавлена!";
                    message.Foreground = Brushes.White;
                    message.Visibility = Visibility.Visible;
                }
            }
            catch (Error error)
            {
                message.Foreground = Brushes.Red;
                message.Text = error.ToString();
                if (message.Visibility == Visibility.Collapsed)
                {
                    message.Visibility = Visibility.Visible;
                }
            }
        }

        private void CloseInTime()
        {
            var timer = new DispatcherTimer();
            timer.Tick += (sender, e) => { Close(); };
            timer.Interval = new TimeSpan(0, 0, 3);
            timer.Start();
        }

        private Installment CreateNewInstallment()
        {
            var installment = new Installment()
            {
                ClientId = (int)client.SelectedValue,
                TourId = (int)tour.SelectedValue,
                Date = dateInstallment.SelectedDate ?? DateTime.Now,
                Period = period.Text.Length != 0 ? Convert.ToInt32(period.Text) : 365
            };

            return installment;
        }

        private void CheckSpaces()
        {
            if (client.SelectedItem is null)
            {
                throw new Error("Ошибка! Клиент не выбран");
            }

            if (tour.SelectedItem is null)
            {
                throw new Error("Ошибка! Тур не выбран");
            }

            if (dateInstallment.SelectedDate is DateTime date)
            {
                if ((date - DateTime.Now).TotalDays < 0)
                {
                    throw new Error("Ошибка! Выберите актуальную дату");
                }
            }

            if(!Sellings.Exists(selling => selling.ClientId == (client.SelectedItem as Client).Id &&
            selling.TourId == (tour.SelectedItem as Tour).Id))
            {
                throw new Error(string.Format($"Ошибка! Клиент {(client.SelectedItem as Client).Name} ранее " +
                    $"не приобретал тур {(tour.SelectedItem as Tour).Name}"));
            }

            if (Installments.Exists(installment => installment.ClientId == (client.SelectedItem as Client).Id &&
            installment.TourId == (tour.SelectedItem as Tour).Id))
            {
                throw new Error(string.Format($"Ошибка! Клиенту {(client.SelectedItem as Client).Name} ранее " +
                    $"уже была предоставлена рассрочка по туру {(tour.SelectedItem as Tour).Name}"));
            }

            if(period.Text.Length != 0 && Convert.ToInt32(period.Text) <= 365)
            {
                throw new Error("Ошибка! Введите срок не превышающий 365-ти дней");
            }


        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
