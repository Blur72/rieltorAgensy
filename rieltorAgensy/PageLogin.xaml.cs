using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Profile;
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
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        public PageLogin()
        {
            InitializeComponent();
        }

        private void txtName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (txtName.Text != "" && txtPass.Text != "")
            {
                buttLog.IsEnabled = true;
            }
            else
            {
                buttLog.IsEnabled = false;
            }
        }

        private void txtPass_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (txtName.Text != "" && txtPass.Text != "")
            {
                buttLog.IsEnabled = true;
            }
            else
            {
                buttLog.IsEnabled = false;
            }
        }

        private void buttLog_Click(object sender, RoutedEventArgs e)
        {
            string LoginClient = txtName.Text;
            string PassClient = txtPass.Password;
            var USER = Class1.dbconnect.Clients.FirstOrDefault(name => name.FirstName == LoginClient);
            var USERPASS = Class1.dbconnect.Clients.FirstOrDefault(pass => pass.LastName == PassClient);
            if (USER == null)
            {
                MessageBox.Show("Пользователь с таким логином не зарегистрирован!");
            }
            else if (USERPASS == null)
            {
                MessageBox.Show("Пользователь с таким паролем не зарегистрирован!");
            }
            else if (AdminCheck.IsChecked == true)
            {
                MainWindow.Instance.Mainframe.Navigate(new PageMain(USER.ClientID));
            }
            else
            {

                string login = txtName.Text;
                string password = txtPass.Password;
                var student = Class1.dbconnect.Clients.FirstOrDefault(log => log.FirstName == login && log.LastName == password);
                var notification = Class1.dbconnect.Notifications.FirstOrDefault(n => n.ClientID == USER.ClientID);

                MessageBox.Show($"Вы успешно вошли, {student.FirstName}");

                if (notification != null)
                {
                    Class1.notifications = notification;
                    Class1.clients = student;
                }
                MainWindow.Instance.Mainframe.Navigate(new ClientPageMain(USER.FirstName));
            }
        }
    }
}
