using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для Tour_card.xaml
    /// </summary>
    public partial class TourCard : Window
    {
        private Mode TourMode { get; set; }
        enum Mode
        {
            Add,
            Edit
        }

        int TourId { get; set; }

        public TourCard()
        {
            InitializeComponent();
            cbMen.ItemsSource = Enumerable.Range(1, 7);
            cbNights.ItemsSource = Enumerable.Range(2, 13);
            allowEverything();
            TourMode = Mode.Add;
        }

        private void allowEverything()
        {
            tbName.IsReadOnly = false;
            tbName.MinWidth = 100;
            tbState.IsReadOnly = false;
            tbCity.IsReadOnly = false;
            cbMen.IsEnabled = true;
            cbNights.IsEnabled = true;
            tbDescription.IsReadOnly = false;
            tbPrice.IsReadOnly = false;
            dpDate.IsEnabled = true;
        }

        public TourCard(Tour tour, Constants.VisitorType visitorType)
        {
            InitializeComponent();
            cbMen.ItemsSource = Enumerable.Range(1, 7);
            cbNights.ItemsSource = Enumerable.Range(2, 13);
            fillSpaces(tour);
            if (visitorType == Constants.VisitorType.Client)
            {
                btnSaver.Visibility = Visibility.Collapsed;
            }
            else
            {
                allowEverything();
            }
            TourMode = Mode.Edit;
            TourId = tour.Id;
        }

        private void fillSpaces(Tour tour)
        {
            tbName.Text = tour.Name;
            tbState.Text = tour.State;
            tbCity.Text = tour.City;
            cbMen.SelectedItem = tour.Men;
            cbNights.SelectedItem = tour.Nights;
            tbDescription.Text = tour.Description;
            tbPrice.Text = tour.Price.ToString("c");
            dpDate.SelectedDate = tour.LastData;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ToursList.TourIsSelected = false;
            Close();
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                checkSpaces();
            }
            catch (Error er)
            {
                message.Text = er.ToString();
                message.Visibility = Visibility.Visible;
                return;
            }
            if (TourMode == Mode.Add)
            { 
                AddTour();
                PrintMessage("Тур успешно добавлен!");
            }
            else
            {
                EditTour();
                PrintMessage("Тур успешно изменён!");
            }

            CloseLater();
        }

        private void EditTour()
        {
            using (var agencyDbContext = new AgencyDbContext())
            {
                Tour tour = agencyDbContext.Tours.Find(TourId);
                tour.Name = tbName.Text;
                    tour.State = tbState.Text;
                tour.City = tbCity.Text;
                tour.Men = (int)cbMen.SelectedItem;
                tour.Nights = (int)cbNights.SelectedItem;
                tour.Description = tbDescription.Text;
                tour.Price = Decimal.Parse(tbPrice.Text,  NumberStyles.Any);
                tour.LastData = dpDate.DisplayDate.AddMonths(1);

                agencyDbContext.SaveChanges();
            }
        }

        private void AddTour()
        {
            using (var agencyDbContext = new AgencyDbContext())
            {
                int maxIndex = agencyDbContext.Tours.ToList().Last().Id + 1;
                Tour tour = new Tour()
                {
                    Id = maxIndex,
                    Name = tbName.Text,
                    State = tbState.Text,
                    City = tbCity.Text,
                    Men = (int)cbMen.SelectedItem,
                    Nights = (int)cbNights.SelectedItem,
                    Description = tbDescription.Text,
                    Price = Convert.ToInt32(tbPrice.Text),
                    LastData = dpDate.DisplayDate.AddMonths(1)
                };
                agencyDbContext.Tours.Add(tour);
                agencyDbContext.SaveChanges();
            }
        }

        private void PrintMessage(string strMessage)
        {
            message.Text = strMessage;
            message.Foreground = Brushes.White;
            message.Visibility = Visibility.Visible;
        }

        private void CloseLater()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (s, ev) =>
            {
                ToursList.TourIsSelected = false;
                Close();
            };
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
        }


        private void checkSpaces()
        {
                if (tbName.Text.Equals("")) { throw new Error("Ошибка! Введите название тура"); }
                if (tbState.Text.Equals("")) { throw new Error("Ошибка! Введите название страны"); }
                if (tbCity.Text.Equals("")) { throw new Error("Ошибка! Введите название города"); }
                if (tbPrice.Text.Equals("")) { throw new Error("Ошибка! Введите цену"); }
                if (dpDate.SelectedDate == null) { throw new Error("Ошибка! Введите последнюю дату работы тура"); }
                if ((dpDate.SelectedDate - DateTime.Now).Value.Days < 26) { throw new Error("Ошибка! Введите последнюю дату работы тура не ранее, чем за 26 дней"); }
                if (cbMen.SelectedItem == null) { throw new Error("Ошибка! Выберите количество человек"); }
                if (cbNights.SelectedItem == null) { throw new Error("Ошибка! Выберите количество ночей"); }           

        }

        private void Border_GotFocus(object sender, RoutedEventArgs e)
        {
            message.Visibility = Visibility.Collapsed;
        }

        private void tbDescription_GotFocus(object sender, RoutedEventArgs e)
        {
            message.Visibility = Visibility.Visible;
        }

        private void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int value;
            if(!Int32.TryParse(e.Text, out value) || tbPrice.Text.Length == 0 && e.Text == "0")
            {
                e.Handled = true;
            }
        }

        private void tbPrice_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
