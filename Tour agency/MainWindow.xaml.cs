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
            mainContent["qwerty"] = new Frame();
            mainContent["awdad"].Content = new Page1();
            mainContent["qwerty"].Content = new Page2();
            foreach (var item in yyy.Items)
            {
                ((TabItem)item).GotFocus += ContextMenuOpening;
            }
        }

        bool stat;

        private void ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = (TabItem)yyy.SelectedItem;
            if(tabItem.Content is null)
            {
                tabItem.Content = mainContent[tabItem.Header.ToString()];
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           /* if (stat)
            {
                if(first is null)
                {
                    first = new Frame();
                    first.Content = new Page1();
                }

                ttt.Content = first;
                stat = !stat;

            }
            else
            {
                if (second is null)
                {
                    second = new Frame();
                    second.Content = new Page2();
                }

                ttt.Content = second;
                stat = !stat;
            }*/
        }
    }
}
