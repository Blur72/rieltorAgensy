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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rieltorAgensy
{
    /// <summary>
    /// Логика взаимодействия для PageRegistration.xaml
    /// </summary>
    public partial class PageRegistration : Page
    {
        public PageRegistration()
        {
            InitializeComponent();
        }

        private void txtName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (txtName.Text != "" && txtPass.Text != "" && Checkboxik.IsChecked == true)
            {
                buttReg.IsEnabled = true;
            }
            else
            {
                buttReg.IsEnabled = false;
            }
        }

        private void txtPass_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (txtName.Text != "" && txtPass.Text != "" && Checkboxik.IsChecked == true)
            {
                buttReg.IsEnabled = true;
            }
            else
            {
                buttReg.IsEnabled = false;
            }
        }

        private void Checkboxik_Checked(object sender, RoutedEventArgs e)
        {
            if (txtName.Text != "" && txtPass.Text != "" && Checkboxik.IsChecked == true)
            {
                buttReg.IsEnabled = true;
            }
            else
            {
                buttReg.IsEnabled = false;
            }
        }

        private void buttReg_Click(object sender, RoutedEventArgs e)
        {
            string LoginClient = txtName.Text;
            var Client = Class1.dbconnect.Clients.FirstOrDefault(name => name.FirstName == LoginClient);
            if (Client != null)
            {
                MessageBox.Show("Клиент с таким логином уже есть!");
            }
            else
            {
                var tempClient = new Clients()
                {
                    FirstName = txtName.Text,
                    LastName = txtPass.Text,
                    PhoneNumber = txtPhone.Text,
                    Email = txtEmail.Text,
                    Address = txtAdres.Text,
                    Balance = 0
                };
                Class1.dbconnect.Clients.Add(tempClient);
                Class1.dbconnect.SaveChanges();
                return;
            }
        }

        private void buttLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.Navigate(new PageLogin());
        }
    }
}
