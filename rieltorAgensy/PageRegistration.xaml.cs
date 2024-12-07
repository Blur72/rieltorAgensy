using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using QRCoder;

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
            QRCode.Source = GenerateQrCodeBitmapImage("https://yandex.ru/images/search?from=tabbar&img_url=https%3A%2F%2Fcdn1.ozone.ru%2Fs3%2Fmultimedia-1-7%2F7005371875.jpg&lr=43&pos=0&rpt=simage&text=%D0%BA%D0%B8%D1%80%D0%B8%D0%B5%D1%88%D0%BA%D0%B8");
        }

        private BitmapImage GenerateQrCodeBitmapImage(string text)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q); using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrBitmap = qrCode.GetGraphic(20))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            qrBitmap.Save(ms, ImageFormat.Png);
                            ms.Position = 0;
                            BitmapImage bitmapImage = new BitmapImage(); bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad; bitmapImage.StreamSource = ms;
                            bitmapImage.EndInit();
                            return bitmapImage;
                        }
                    }
                }
            }
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
                    LastName = txtPass.Password,
                    PhoneNumber = txtPhone.Text,
                    Email = txtEmail.Text + pochta.Text,
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
