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

        public MainWindow()
        {
            InitializeComponent();

            mainContent = new Dictionary<String, Frame>();
            mainContent["awdad"] = new Frame();
            mainContent["awdad"].Content = new Page1();

            ttt.Content = mainContent["awdad"];
          
            mainContent["qwerty"] = new Frame();

            mainContent["qwerty"].Content = new Page2();

            foreach (var item in yyy.Items)
            {
                TabItem tItem = (TabItem)item;
                tItem.GotFocus += ContextMenuOpening;
                tItem.Background = Brushes.Transparent;
                
            }
        }

        private void ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = (TabItem)yyy.SelectedItem;
            if(tabItem.Content is null)
            {
                tabItem.Content = mainContent[tabItem.Header.ToString()];
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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
