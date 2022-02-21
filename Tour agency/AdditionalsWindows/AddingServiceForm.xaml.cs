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
using System.Windows.Shapes;
using System.Windows.Threading;
using Tour_agency.Model;

namespace Tour_agency.AdditionalsWindows
{
    /// <summary>
    /// Логика взаимодействия для AddingService.xaml
    /// </summary>
    public partial class AddingServiceForm : Window
    {
        List<Selling> Sellings { get; set; }
        int VisitorId { get; set; }

        public AddingServiceForm(int visitorId)
        {
            InitializeComponent();
            VisitorId = visitorId;

            using(var agencyDbContext = new AgencyDbContext())
            {
                Sellings = agencyDbContext.Sellings.ToList().Where(selling => selling.ManagerId == VisitorId).ToList();
                var clientIds = Sellings.Select(selling => selling.ClientId).Distinct().ToList();
                client.ItemsSource = agencyDbContext.Clients.ToList().Where(item => clientIds.Contains(item.Id));
                var tourIds = Sellings.Select(selling => selling.TourId).Distinct().ToList();
                tour.ItemsSource = agencyDbContext.Tours.ToList().Where(item => tourIds.Contains(item.Id));
                service.ItemsSource = agencyDbContext.Services.ToList();
            }

            ServicesList.AddingService = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ServicesList.AddingService = false;
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SellService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckSpaces();
                using (var agencyDbContext = new AgencyDbContext())
                {
                    var addingService = CreateNewAddingService();
                    agencyDbContext.AddingServices.Add(addingService);
                    agencyDbContext.SaveChanges();
                    sellService.IsEnabled = false;
                    CloseInTime();
                    message.Text = "Услуга успешно продана!";
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
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            ServicesList.AddingService = false;
        }

        private AddingService CreateNewAddingService()
        {
            var addingService = new AddingService()
            {
                ClientId = (int)client.SelectedValue,
                TourId = (int)tour.SelectedValue,
                ServiceId = (int)service.SelectedValue
            };
            return addingService;
        }

        private void CheckSpaces()
        {
            if (client.SelectedItem is null)
            {
                throw new Error("Ошибка! Выберите клиента");
            }

            if (tour.SelectedItem is null)
            {
                throw new Error("Ошибка! Выберите тур");
            }

            if (service.SelectedItem is null)
            {
                throw new Error("Ошибка! Выберите услугу");
            }

            if (!Sellings.Exists(selling => selling.ClientId == (int)client.SelectedValue &&
            selling.TourId == (int)tour.SelectedValue))
            {
                throw new Error(string.Format($"Ошибка! {(client.SelectedItem as Client).Name} не" +
                    $" покупал/а тур {(tour.SelectedItem as Tour).Name} "));
            }

            if (IsReplay())
            {
                throw new Error("Ошибка! Данная услуга уже была добавлена");
            }

        }
        
        private bool IsReplay()
        {
            using(var agencyDbContext = new AgencyDbContext())
            {
                return agencyDbContext.AddingServices.ToList().Exists(adding => adding.ClientId == (int)client.SelectedValue &&
            adding.TourId == (int)tour.SelectedValue && adding.ServiceId == (int)service.SelectedValue);
            }
        }

        private void StackPanel_GotFocus(object sender, RoutedEventArgs e)
        {
            message.Visibility = Visibility.Collapsed;
        }
    }
}
