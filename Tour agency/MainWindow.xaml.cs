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
        Dictionary<String, Frame> mainContent;
        Constants.VisitorType TypeOfVisitor = Constants.VisitorType.Client;
        int VisitorId = 1;

    /*    public MainWindow(Constants.AccountId accountId)
        {
        }*/

            public MainWindow(/*Constants.AccountId accountId*/)
        {
            InitializeComponent();
       //     this.accountId = /*accountId;*/ new Constants.AccountId
       //     {
   /*             id = 1,
                visitorType = Constants.AccountId.VisitorType.Client
            };*/

            mainContent = new Dictionary<String, Frame>();
            mainContent["Список туров"] = new Frame();
            mainContent["Список туров"].Content = new ToursList();

            ttt.Content = mainContent["Список туров"];
            mainContent["Список туров"].MouseDown -= Grid_MouseDown;


            mainContent["Список услуг"] = new Frame();

            mainContent["Список услуг"].Content = new ServicesList();
            mainContent["Список рассрочек"] = new Frame();

            mainContent["Список рассрочек"].Content = new InstallmentsList(TypeOfVisitor, VisitorId);

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
            Close();
        }
    }
}
