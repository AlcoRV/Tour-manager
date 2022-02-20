using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
using Tour_agency.AdditionalsWindows;
using Tour_agency.Model;

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class ServicesList : Page
    {
        public static bool AddingService { get; set; } = false;

        public ServicesList(Constants.VisitorType visitorType, int visitorId)
        {
            InitializeComponent();
            fixedServices = new List<Service>();
            addedServices = new List<Service>();
            TypeOfVisitor = visitorType;
            VisitorId = visitorId;
            if(TypeOfVisitor == Constants.VisitorType.Client)
            {
                btnSaver.Visibility = Visibility.Collapsed;
                table.CanUserAddRows = false;
                table.IsReadOnly = true;
                btnAddingService.Visibility = Visibility.Collapsed;
            }
        }

        Constants.VisitorType TypeOfVisitor { get; set; }
        int VisitorId { get; set; }
        private List<Service> fixedServices { get; set; }
        private List<Service> addedServices { get; set; }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using(AgencyDbContext agencyDbContext = new AgencyDbContext())
            {
                table.ItemsSource = agencyDbContext.Services.ToList();
            }
            table.MouseDown += (s, ev) => { };
        }

        private void btnServises_Click(object sender, RoutedEventArgs e)
        {

            using (var agencyDbContext = new AgencyDbContext())
            {
                foreach(var item in addedServices)
                {
                    agencyDbContext.Services.Add(item);
                }
                

               foreach (var item in fixedServices)
               {
                   Service service = agencyDbContext.Services.Find(item.Id);
                   service.Name = item.Name;
                   service.Price = item.Price;
               }
                
                agencyDbContext.SaveChanges();
                table.ItemsSource = agencyDbContext.Services.ToList();
            }
        }

        private void table_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.Column.DisplayIndex == 2)
                {
                    CheckPrice(e);
                }
                else if (e.Column.DisplayIndex == 1)
                {
                    CheckName(e);
                }
                e.Row.Background = Brushes.White;
                (e.EditingElement as TextBox).Background = Brushes.White;
                message.Visibility = Visibility.Collapsed;
                btnSaver.IsEnabled = true;
            }
            catch(Error error)
            {
                message.Text = error.ToString();
                (e.EditingElement as TextBox).Background = Brushes.Red;
                e.Row.Background = Brushes.Red;
                message.Visibility = Visibility.Visible;
                btnSaver.IsEnabled = false;
            }
        }

        private void CheckName(DataGridCellEditEndingEventArgs e)
        {
            var element = e.EditingElement as TextBox;
            if (element.Text.Length == 0)
            {
                throw new Error("Ошибка! Отсутствует название");
            }
            else
            {
                Service service = e.Row.Item as Service;
                if (!fixedServices.Contains(service))
                {
                    fixedServices.Add(service);
                }
            }
        }

        private void CheckPrice(DataGridCellEditEndingEventArgs e)
        {
            decimal price;
            var element = e.EditingElement as TextBox;
            if (Decimal.TryParse(element.Text, System.Globalization.NumberStyles.Any, null, out price))
            {
                Service service = e.Row.Item as Service;
                if (!fixedServices.Contains(service))
                {
                    fixedServices.Add(service);
                }
            }
            else
            {
                throw new Error("Ошибка! Введите корректное значение цены");
            }
        }

        private void table_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {

            int maxIndex = (table.Items.SourceCollection as List<Service>).Last().Id + 1;
                e.NewItem = new Service()
                {
                    Id = maxIndex,
                };
            addedServices.Add(e.NewItem as Service);
        }

        private void table_KeyDown(object sender, KeyEventArgs e)
        {
            if (TypeOfVisitor == Constants.VisitorType.Client) { return; }
            if (e.Key == Key.Delete)
            {
                int serviceId = (table.SelectedItem as Service).Id;
                using (var agencyDbContext = new AgencyDbContext())
                {
                    Service service = agencyDbContext.Services.Find(serviceId);
                    if (service != null)
                    {
                        agencyDbContext.Services.Remove(service);
                        agencyDbContext.SaveChanges();
                    }
                    
                }
                Service deletingService = table.CurrentItem as Service;
                if (fixedServices.Contains(deletingService))
                {
                    fixedServices.Remove(deletingService);
                }
                if (addedServices.Contains(deletingService))
                {
                    addedServices.Remove(deletingService);
                }
            }
            
        }

        private void addingServicesTable_Loaded(object sender, RoutedEventArgs e)
        {
            using(var agencyDbContext = new AgencyDbContext())
            {
                addingServicesTable.ItemsSource = agencyDbContext.AddingServices.ToList();
                foreach(var item in addingServicesTable.Items)
                {
                    Model.AddingService addingServices = item as Model.AddingService;
                    addingServices.Client = agencyDbContext.Clients.Find(addingServices.ClientId);
                    addingServices.Tour = agencyDbContext.Tours.Find(addingServices.TourId);
                    addingServices.Service = agencyDbContext.Services.Find(addingServices.ServiceId);
                }
            }

            if(TypeOfVisitor == Constants.VisitorType.Client)
            {
                addingServicesTable.Columns[1].Visibility = Visibility.Collapsed;
                addingServicesTable.Items.Filter = item => (item as AddingService).ClientId == VisitorId;
            }

        }

        private void btnAddingService_Click(object sender, RoutedEventArgs e)
        {
            if (AddingService) { return; }

            var addingServiceWindow = new AddingServiceForm();
            addingServiceWindow.Show();
        }
    }

}
