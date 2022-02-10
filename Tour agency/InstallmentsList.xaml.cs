using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tour_agency.Model;

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для InstallmentsList.xaml
    /// </summary>
    public partial class InstallmentsList : Page
    {
        Constants.VisitorType TypeOfVisitor { get; }
        int VisitorId { get; }

        public InstallmentsList(Constants.VisitorType visitorType, int visitorId)
        {
            InitializeComponent();
            TypeOfVisitor = visitorType;
            VisitorId = visitorId;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var agencyDbContext = new AgencyDbContext())
            {
                tableSelling.ItemsSource = agencyDbContext.sellings.ToList();
                if(TypeOfVisitor == Constants.VisitorType.Client)
                {
              //      tableSelling.Items.Filter = item => (item as Selling). == /\/\/\/\отсев по клиенту
                }
            }
        }
    }
}
