using System;
using System.Collections.Generic;
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

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            login.GotFocus += GotFocus;
            password.GotFocus += GotFocus;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            try
            {

                CheckDataForSpace();

                Constants.VisitorType status;
                int? accountId;

                using (AgencyDbContext agencyDbContext = new AgencyDbContext())
                {

                    (status, accountId) = agencyDbContext.GetVisitorData(login.Text, password.Password);

                }

                if (status.Equals(Constants.VisitorType.Error))
                {
                    throw new Error("Неверный логин или пароль!");
                }
                else
                {
                    MainWindow mainWindow = new MainWindow(status, (int)accountId);
                    mainWindow.Show();
                    Close();
                }
            }
            catch (Error err)
            {
                error.Text = err.ToString();
                error.Visibility = Visibility.Visible;
            }

        }

        private void CheckDataForSpace()
        {
            if(login.Text.Length == 0)
            {
                throw new Error("Не введён логин!");
            }
            if (password.Password.Length == 0)
            {
                throw new Error("Не введён пароль!");
            }

        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private new void GotFocus(object sender, RoutedEventArgs e)
        {
            if(error.Visibility == Visibility.Visible)
            {
                error.Visibility = Visibility.Collapsed;
            }
        }
    }
}
