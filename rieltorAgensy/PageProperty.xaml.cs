using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Data.Entity;
using System.Windows.Media.Media3D;
using Wpf.Ui;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace rieltorAgensy
{
    /// <summary>
    /// Логика взаимодействия для PageProperty.xaml
    /// </summary>
    public partial class PageProperty : Page
    {
        public PageProperty()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string address = adres.Text;
            string propType = TypeProp.Text;
            int pr = Convert.ToInt32(priceText.Text);
            int propsize = Convert.ToInt32(area.Text);
            int bed = Convert.ToInt32(numBed.Text);
            int bath = Convert.ToInt32(numBath.Text);
            string desc = descText.Text;
            int idst = Convert.ToInt32(idStatus.Text);
            int idty = Convert.ToInt32(idType.Text);

            var client = Class1.dbconnect.Propertiezzz.FirstOrDefault(adr1 => adr1.Address == address);

            if (adres.Text == null || TypeProp.Text == null || priceText.Text == null || area.Text == null || numBed.Text == null || numBath.Text == null || descText.Text == null || idStatus.Text == null || idType.Text == null)
            {
                MessageBox.Show("Вы не заполнили обязательные поля!");
                return;
            }
            else if (client != null)
            {
                MessageBox.Show("Такой клиент уже есть");
            }
            else
            {
                var tempProperty = new Propertiezzz()
                {
                    Address = adres.Text,
                    PropertyType = TypeProp.Text,
                    Price = Convert.ToInt32(priceText.Text),
                    Size = Convert.ToInt32(area.Text),
                    Bedrooms = Convert.ToInt32(numBed.Text),
                    Bathrooms = Convert.ToInt32(numBath.Text),
                    Description = descText.Text,
                    PropertyStatusID = Convert.ToInt32(idStatus.Text),
                    PropertyTypeID = Convert.ToInt32(idType.Text)
                };
                Class1.dbconnect.Propertiezzz.Add(tempProperty);
                Class1.dbconnect.SaveChanges();
                MessageBox.Show("Недвижимость сохранена!");
                FlightsList_Loaded(sender, e);
            }
        }


        private string connectionString = "Server=DESKTOP-LSOM170\\SQLEXPRESS;Database=RieltorskoeAgentsvo;Integrated Security=True;";
        private void FlightsList_Loaded(object sender, RoutedEventArgs e)
        {
            var airports = Class1.dbconnect.Propertiezzz.ToList();

            // Проверяем, что airports не пустой
            if (airports != null && airports.Count > 0)
            {
                var flightList = airports.Select(flight => new Propertiezzz
                {
                    Address = flight.Address,
                    PropertyType = TypeProp.Text, // Убедитесь, что это значение корректное
                    Price = flight.Price != null ? Convert.ToInt32(flight.Price) : 0, // Обработка null
                    Size = flight.Size != null ? Convert.ToInt32(flight.Size) : 0, // Обработка null
                    Bedrooms = flight.Bedrooms != null ? Convert.ToInt32(flight.Bedrooms) : 0, // Обработка null
                    Description = flight.Description,
                    Bathrooms = flight.Bathrooms != null ? Convert.ToInt32(flight.Bathrooms) : 0, // Обработка null
                    PropertyStatusID = int.TryParse(idStatus.Text, out int statusID) ? statusID : 0, // Обработка ввода
                    PropertyTypeID = int.TryParse(idType.Text, out int typeID) ? typeID : 0 // Обработка ввода
                }).ToList();

                FlightsList.ItemsSource = flightList;
            }
            else
            {
                // Обработка случая, если airports пустой
                FlightsList.ItemsSource = null;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (FlightsList.SelectedItem != null)
            {
                Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;
                Class1.dbconnect.Propertiezzz.Remove(selectedProp);
                Class1.dbconnect.SaveChanges();
                FlightsList.ItemsSource = Class1.dbconnect.Propertiezzz.ToList();
                return;
            }
            else
            {
                MessageBox.Show("Недвижимость для удаления не была выбрана");
            }
        }
    }
}

