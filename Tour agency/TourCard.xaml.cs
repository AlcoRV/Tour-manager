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
using Tour_agency.Model;

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для Tour_card.xaml
    /// </summary>
    public partial class TourCard : Window
    {
        public TourCard(Tour tour)
        {
            InitializeComponent();
            cbMen.ItemsSource = Enumerable.Range(1, 7);
            cbNights.ItemsSource = Enumerable.Range(2, 13);
            fillSpaces(tour);
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

        private void tbPrice_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
