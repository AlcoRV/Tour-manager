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

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для InstallmentsList.xaml
    /// </summary>
    public partial class InstallmentsList : Page
    {
        public InstallmentsList()
        {
            InitializeComponent();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-GJS201Q\SQLEXPRESS;Initial Catalog=ТурАгентство;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM РассрочкиДля(@НомерКлиента)";
                    command.Parameters.Add("@НомерКлиента", SqlDbType.Int);
                    command.Parameters["@НомерКлиента"].Value = 2;

                    connection.Open();

                    var tab = new DataTable();
                    tab.Load(command.ExecuteReader());
                    table.DataContext = tab.DefaultView;

                    connection.Close();
                }
            }
        }
    }
}
