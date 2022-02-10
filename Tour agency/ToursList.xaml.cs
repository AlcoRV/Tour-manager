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
        public ToursList()
        {
            InitializeComponent();
        }

        void filter()
        {

            table.Items.Filter = item => (item as Tour).State.StartsWith(tbState.Text) && //expression for state
            (item as Tour).City.StartsWith(tbCity.Text) &&                                  //expression for city
            (cbNights.SelectedItem?.Equals((item as Tour).Nights) ?? true) &&               //expression for nights
            (cbMen.SelectedItem?.Equals((item as Tour).Men) ?? true) &&                      //expression for men
            (tbPriceFrom.Text == "" ? true : (item as Tour).Price >= Convert.ToInt32(tbPriceFrom.Text)) &&  //expression for price
            (tbPriceTo.Text == "" ? true : (item as Tour).Price <= Convert.ToInt32(tbPriceTo.Text));       //

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var agencyDbContext = new AgencyDbContext())
            {
                table.ItemsSource = agencyDbContext.Tours.ToList();
            }
            cbNights.ItemsSource = Enumerable.Range(1, 14);
            cbMen.ItemsSource = Enumerable.Range(1, 6);

            /*table.MouseDoubleClick += (send, ev) => {
                var item = table.SelectedItem;
                ((DataRowView)item).Row["Номер"] = 50;

            };*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            filter();
        }
    }
}
