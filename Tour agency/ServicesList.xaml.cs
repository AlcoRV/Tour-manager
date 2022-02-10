﻿using System;
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
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class ServicesList : Page
    {
        public ServicesList()
        {
            InitializeComponent();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using(AgencyDbContext agencyDbContext = new AgencyDbContext())
            {
                table.ItemsSource = agencyDbContext.services.ToList();
            }
        }
    }
}
