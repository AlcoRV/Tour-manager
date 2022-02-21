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

        public static bool TourIsSelling { get; set; } = false;
        public static bool InstallmentAreIssued { get; set; } = false;

        public InstallmentsList(Constants.VisitorType visitorType, int visitorId)
        {
            InitializeComponent();
            TypeOfVisitor = visitorType;
            VisitorId = visitorId;
            
            using(var agencyDbContext = new AgencyDbContext())
            {
                yyy.ItemsSource = agencyDbContext.Tours.ToList().Select(item => item.Name);
                www.ItemsSource = agencyDbContext.Clients.ToList().Select(item => item.Name);
            }

            if (TypeOfVisitor == Constants.VisitorType.Manager)
            {
                www.Visibility = Visibility.Visible;
                installmentTable.CanUserAddRows = true;
                tableSelling.CanUserAddRows = true;
                btnInstallment.Visibility = Visibility.Visible;
                btnSelling.Visibility = Visibility.Visible;
            }
        }

        private void tableSelling_Loaded(object sender, RoutedEventArgs e)
        {
            using (var agencyDbContext = new AgencyDbContext())
            {
                if(TypeOfVisitor == Constants.VisitorType.Client)
                {
                    tableSelling.ItemsSource = agencyDbContext.Sellings.ToList().Where(item => item.ClientId == VisitorId).ToList();

                    foreach (var item in tableSelling.Items)
                    {
                        Selling selling = item as Selling;
                        if (selling is null) { break; }
                        selling.Manager = agencyDbContext.Managers.Find(selling.ManagerId);
                        selling.Tour = agencyDbContext.Tours.Find(selling?.TourId);
                    }

                }
                else
                {
                    tableSelling.ItemsSource = agencyDbContext.Sellings.ToList().Where(item => item.ManagerId == VisitorId).ToList();

                    foreach (var item in tableSelling.Items)
                    {
                        Selling selling = item as Selling;
                        if (selling is null) { break; }
                        selling.Client = agencyDbContext.Clients.Find(selling.ClientId);
                        selling.Tour = agencyDbContext.Tours.Find(selling?.TourId);
                    }
                    AdaptToManager();
                }

            }

        }

        private void AdaptToManager()
        {
            sellingLabel.Text = "Продажи";
            nameColumn.Header = "ФИО клиента";
            nameColumn.Binding = new Binding() { Path = new PropertyPath("Client.Name") };
            tableSelling.Items.Filter = item => (item as Selling).ManagerId == VisitorId;
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
            }

            if (TypeOfVisitor == Constants.VisitorType.Client)
            {
                installmentTable.Items.Filter = item => (item as Installment).ClientId == VisitorId;
            }
            else
            {
                installmentTable.Items.Filter = item => tableSelling.Items.OfType<Selling>().ToList().Exists(el =>
                el.TourId == (item as Installment).TourId && el.ClientId == (item as Installment).ClientId);
            }

        }

        private void newSelling_Click(object sender, RoutedEventArgs e)
        {
            if (TourIsSelling == true) { return; }

            using (var agencyDbContext = new AgencyDbContext())
            {
                var newSelling = new FormNewSelling(agencyDbContext.Sellings.ToList(),
                    agencyDbContext.Managers.ToList().Find(manager => manager.Id == VisitorId) as Manager);
                newSelling.Show();
            }
        }

        private void newInstallment_Click(object sender, RoutedEventArgs e)
        {
            if(InstallmentAreIssued == true) { return; }

            using (var agencyDbContext = new AgencyDbContext())
            {
                var newInstallment = new FormNewInstallment(agencyDbContext.Sellings.ToList(), 
                    agencyDbContext.Installments.ToList(), VisitorId);
                newInstallment.Show();
            }
        }
    }
}
