﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum Status { log_in, register }
        private Status status { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            SetStatus(Status.register);
        }

        private void Log_in(object sender, RoutedEventArgs e)
        {
            SetStatus(Status.log_in);
        }

        private void SetStatus(Status status)
        {
            if (this.status.Equals(status))
            {
                return;
            }
            else if (status.Equals(Status.log_in))
            {
                CheckPass.Visibility = Visibility.Collapsed;
                Email.Visibility = Visibility.Collapsed;
                btnLogin.FontWeight = FontWeights.Bold;
                btnRegister.FontWeight = FontWeights.Normal;
                this.status = Status.log_in;
            }
            else
            {
                CheckPass.Visibility = Visibility.Visible;
                Email.Visibility = Visibility.Visible;
                btnLogin.FontWeight = FontWeights.Normal;
                btnRegister.FontWeight = FontWeights.Bold;
                this.status = Status.register;
            }
        }
    }
}