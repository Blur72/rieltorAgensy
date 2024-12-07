using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Логика взаимодействия для PageProfile.xaml
    /// </summary>
    public partial class PageProfile : Page
    {

        string firstname;

        public PageProfile(string FIRSTNAME)
        {
            InitializeComponent();
            this.firstname = FIRSTNAME;
            var tempClient = Class1.dbconnect.Clients.FirstOrDefault(cli => cli.FirstName == firstname);
            txtBalance.Text = Convert.ToString(tempClient.Balance);
            txtName.Text = tempClient.FirstName;
        }

        private void NotificationList_Loaded(object sender, RoutedEventArgs e)
        {
            Clients client = Class1.clients;

            if (client == null)
            {
                MessageBox.Show("Вы вошли!");
                return;
            }

            if (Class1.dbconnect == null)
            {
                MessageBox.Show("Ошибка подключения к базе данных.");
                return;
            }

            var notifications = Class1.dbconnect.Notifications
                .Where(n => n.ClientID == client.ClientID)
                .ToList();

            if (notifications.Count == 0)
            {
                NotificationList.ItemsSource = null; 
                MessageBox.Show("У данного клиента нет уведомлений.");
            }
            else
            {
                NotificationList.ItemsSource = notifications;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.GoBack();
        }

        private void butBalance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (summ.Text != null && Convert.ToInt32(summ.Text) > 0)
                {
                    var tempClient = Class1.dbconnect.Clients.FirstOrDefault(cli => cli.FirstName == firstname);
                    tempClient.Balance += Convert.ToInt32(summ.Text);
                    txtBalance.Text = Convert.ToString(tempClient.Balance);
                    Class1.dbconnect.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Вы не ввели сумму либо она меньше 0");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
