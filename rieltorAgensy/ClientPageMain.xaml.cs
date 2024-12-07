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

namespace rieltorAgensy
{
    /// <summary>
    /// Логика взаимодействия для ClientPageMain.xaml
    /// </summary>
    public partial class ClientPageMain : Page
    {
        string firstname;
        public ClientPageMain(string FIRSTNAME)
        {
            InitializeComponent();
            this.firstname = FIRSTNAME;
            var tempClient = Class1.dbconnect.Clients.FirstOrDefault(cli => cli.FirstName == firstname);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.Navigate(new ClientPageProperty(firstname));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.Navigate(new PageProfile(firstname));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.GoBack();
        }

    }
}
