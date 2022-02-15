using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tour_agency.Model;

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class ToursList : Page
    {
        public static bool TourIsSelected { get; set; } = false;
        public Constants.VisitorType TypeOfVisitor { get; set; }

        public ToursList(Constants.VisitorType visitorType)
        {
            InitializeComponent();
            TypeOfVisitor = visitorType;
        }
        void filter()
        {

            table.Items.Filter = item => (item as Tour).State.StartsWith(tbState.Text) && //expression for state
            (item as Tour).City.StartsWith(tbCity.Text) &&                                  //expression for city
            (cbNights.SelectedItem?.Equals((item as Tour).Nights.ToString()) ?? true) &&               //expression for nights
            (cbMen.SelectedItem?.ToString().Equals((item as Tour).Men.ToString()) ?? true) &&                      //expression for men
            (tbPriceFrom.Text == "" ? true : (item as Tour).Price >= Convert.ToInt32(tbPriceFrom.Text)) &&  //expression for price
            (tbPriceTo.Text == "" ? true : (item as Tour).Price <= Convert.ToInt32(tbPriceTo.Text));       //

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            using (var agencyDbContext = new AgencyDbContext())
            {
                if (TypeOfVisitor == Constants.VisitorType.Client)
                {
                    table.ItemsSource = agencyDbContext.Tours.ToList().Where(tour => (tour.LastData - DateTime.Now).Days > 26);
                }
                else
                {
                    table.ItemsSource = agencyDbContext.Tours.ToList();
                }

            }
            var content = new List<string> { "" };
            content.AddRange(Enumerable.Range(2, 13).Select(x => x.ToString()));
            cbNights.ItemsSource = content;

            content = new List<string> { "" };
            content.AddRange(Enumerable.Range(1, 6).Select(x => x.ToString()));
            cbMen.ItemsSource = content;

            table.MouseDoubleClick += (send, ev) => {
                var item = table.SelectedItem;
                Tour tour = item as Tour;
                if(TourIsSelected == false)
                {
                    TourIsSelected = true;
                    var tourCard = new TourCard(tour);
                    tourCard.Show();
                }
            };
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            filter();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (TourIsSelected == false)
            {
                TourIsSelected = true;
                var tourCard = new TourCard();
                tourCard.Show();
            }
        }

        private void cbNights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbNights.SelectedItem == null) { return; }
            if (cbNights.SelectedItem.ToString().Equals("")) { cbNights.SelectedItem = null; }
        }

        private void cbMen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMen.SelectedItem == null) { return; }
            if (cbMen.SelectedItem.ToString().Equals("")) { cbMen.SelectedItem = null; }
        }
    }
}
