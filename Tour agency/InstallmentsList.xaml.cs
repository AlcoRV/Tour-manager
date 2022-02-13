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

        private void tableSelling_Loaded(object sender, RoutedEventArgs e)
        {
            using (var agencyDbContext = new AgencyDbContext())
            {
                tableSelling.ItemsSource = agencyDbContext.Sellings.ToList();
                foreach(var item in tableSelling.Items)
                {
                    Selling selling = item as Selling;
                    if(selling is null) { break; }
                    selling.Manager = agencyDbContext.Managers.Find(selling.ManagerId);
                    selling.Client = agencyDbContext.Clients.Find(selling?.ClientId);
                    selling.Tour = agencyDbContext.Tours.Find(selling?.TourId);
                }



                if(TypeOfVisitor == Constants.VisitorType.Client)
                {
                    tableSelling.Items.Filter = item => (item as Selling).ClientId == VisitorId;
                }
                else
                {
                    sellingLabel.Text = "Продажи";
                    nameColumn.Header = "ФИО клиента";
                    nameColumn.Binding = new Binding() { Path = new PropertyPath("Client.Name") };
                    tableSelling.Items.Filter = item => (item as Selling).ManagerId == VisitorId;
                }

            }
        }

        private void installmentTable_Loaded(object sender, RoutedEventArgs e)
        {
            using(var agencyDbContent = new AgencyDbContext())
            {
                installmentTable.ItemsSource = agencyDbContent.Installments.ToList();

                foreach (var item in installmentTable.Items)
                {
                    Installment installment = item as Installment;
                    if (installment is null) { break; }
                    installment.Client = agencyDbContent.Clients.Find(installment.ClientId);
                    installment.Tour = agencyDbContent.Tours.Find(installment.TourId);
                }

                if (TypeOfVisitor == Constants.VisitorType.Client)
                {
                    installmentTable.Items.Filter = item => (item as Installment).ClientId == VisitorId;
                }
                else
                {
                    DataGridTextColumn column = new DataGridTextColumn()
                    {
                        Header = "ФИО клиента",
                        Binding = new Binding("Client.Name")
                    };
                    installmentTable.Columns.Add(column);
                    column.DisplayIndex = 0;
                }

            }
        }
    }
}
