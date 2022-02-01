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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class ToursList : Page
    {
        public ToursList()
        {
            InitializeComponent();
            FillTable();
            table.MouseDoubleClick += (sender, e) => {
                var item = table.SelectedItem;
                ((DataRowView)item).Row["Номер"] = 50;
                    
            };
            table.AutoGeneratingColumn += (sender, e) =>
            {
                if(e.PropertyType == typeof(System.DateTime))
                {
                    (e.Column as DataGridTextColumn).Binding.StringFormat = "d";
                }
            };
        }


        private void FillTable()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-GJS201Q\SQLEXPRESS;Initial Catalog=ТурАгентство;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM АктуальныеТуры";

                connection.Open();

                var tab = new DataTable();
                tab.Load(command.ExecuteReader());
                table.DataContext = tab.DefaultView;

                connection.Close();

            }


            
        }
    }
}
