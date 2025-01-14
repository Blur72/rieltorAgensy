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

namespace rieltorAgensy
{
    /// <summary>
    /// Логика взаимодействия для PageMain.xaml
    /// </summary>
    public partial class PageMain : Page
    {
        int clientid;
        public PageMain(int clientid)
        {
            InitializeComponent();
            this.clientid = clientid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.Navigate(new PageProperty(clientid));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.Navigate(new PageDeals());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.GoBack();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.Navigate(new PageReports());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.Navigate(new PageAnalisys());
        }
    }
}
