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

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static bool PersonalDataIsShowing { get; set; } = false;
        Dictionary<String, Frame> mainContent;
        Constants.VisitorType TypeOfVisitor { get; set; } = Constants.VisitorType.Manager;
        int VisitorId { get; set; } = 7;

        public MainWindow(Constants.VisitorType visitorType, int accountId) { }

            public MainWindow(/*Constants.VisitorType visitorType, int accountId*/)
        {
            InitializeComponent();
        ///    VisitorId = accountId;
           //TypeOfVisitor = visitorType;

            mainContent = new Dictionary<String, Frame>();
            mainContent["Список туров"] = new Frame();
            mainContent["Список туров"].Content = new ToursList(TypeOfVisitor);

            ttt.Content = mainContent["Список туров"];
            mainContent["Список туров"].MouseDown -= Grid_MouseDown;
            

            mainContent["Услуги"] = new Frame();

            mainContent["Услуги"].Content = new ServicesList(TypeOfVisitor, VisitorId);
            mainContent["Продажи и рассрочки"] = new Frame();

            mainContent["Продажи и рассрочки"].Content = new InstallmentsList(TypeOfVisitor, VisitorId);

            foreach (var item in yyy.Items)
            {
                TabItem tItem = (TabItem)item;
                tItem.GotFocus += ContextMenuOpening;
                tItem.Background = Brushes.Transparent;
            }
        }

        private new void ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = (TabItem)yyy.SelectedItem;
            if(tabItem.Content is null)
            {
                tabItem.Content = mainContent[tabItem.Header.ToString()];
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnPersonalData_Click(object sender, RoutedEventArgs e)
        {
            if(PersonalDataIsShowing) { return; }
            var personalData = new PersonalData(TypeOfVisitor, VisitorId);
            personalData.Show();
        }
    }
}
