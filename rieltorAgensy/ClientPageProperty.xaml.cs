using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MsgReader.Outlook;
using QRCoder;

namespace rieltorAgensy
{
    /// <summary>
    /// Логика взаимодействия для ClientPageProperty.xaml
    /// </summary>
    public partial class ClientPageProperty : Page
    {

        string firstname;
        public ClientPageProperty(string FIRSTNAME)
        {
            InitializeComponent();
            LoadData();
            this.firstname = FIRSTNAME;
            var tempClient = Class1.dbconnect.Clients.FirstOrDefault(cli => cli.FirstName == firstname);
            balanceTxt.Text = Convert.ToString(tempClient.Balance);
        }

        private string connectionString = "Server=DESKTOP-LSOM170\\SQLEXPRESS;Database=RieltorskoeAgentsvo;Integrated Security=True;";


        private void PropertyTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterProperties();
        }

        private void PropertyStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterProperties();
        }

        private void FilterProperties()
        {
            int? selectedTypeID = PropertyTypeComboBox.SelectedValue as int?;
            int? selectedStatusID = PropertyStatusComboBox.SelectedValue as int?;

            List<Propertiezzz> properties = LoadAllProperties();
            List<Propertiezzz> filteredProperties = new List<Propertiezzz>();

            foreach (var property in properties)
            {
                bool matchesType = !selectedTypeID.HasValue || property.PropertyTypeID == selectedTypeID.Value;
                bool matchesStatus = !selectedStatusID.HasValue || property.PropertyStatusID == selectedStatusID.Value;

                if (matchesType && matchesStatus)
                {
                    filteredProperties.Add(property);
                }
            }

            FlightsList.ItemsSource = filteredProperties;
        }

        private List<Propertiezzz> LoadAllProperties()
        {
            List<Propertiezzz> properties = new List<Propertiezzz>();
            string connectionString = "Server=DESKTOP-LSOM170\\SQLEXPRESS;Database=RieltorskoeAgentsvo;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PropertyID, Address, PropertyType, Price, Size, Bedrooms, Bathrooms, Description, PropertyStatusID, PropertyTypeID FROM Propertiezzz";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            properties.Add(new Propertiezzz
                            {
                                PropertyID = reader["PropertyID"] != DBNull.Value ? Convert.ToInt32(reader["PropertyID"]) : 0,
                                Address = reader["Address"] != DBNull.Value ? (string)reader["Address"] : string.Empty,
                                PropertyType = reader["PropertyType"] != DBNull.Value ? (string)reader["PropertyType"] : string.Empty,
                                Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0m,
                                Size = reader["Size"] != DBNull.Value ? Convert.ToInt32(reader["Size"]) : 0,
                                Bedrooms = reader["Bedrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bedrooms"]) : 0,
                                Bathrooms = reader["Bathrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bathrooms"]) : 0,
                                Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty,
                                PropertyStatusID = reader["PropertyStatusID"] != DBNull.Value ? Convert.ToInt32(reader["PropertyStatusID"]) : 0,
                                PropertyTypeID = reader["PropertyTypeID"] != DBNull.Value ? Convert.ToInt32(reader["PropertyTypeID"]) : 0
                            });
                        }
                    }
                }
            }
            return properties;
        }

        private List<Propertiezzz> allProperties;

        private void LoadData()
        {
            allProperties = LoadAllProperties();

            List<PropertyType> propertyTypes = LoadPropertyTypes();
            List<PropertyStatus> propertyStatuses = LoadPropertyStatuses();

            PropertyTypeComboBox.ItemsSource = propertyTypes;
            PropertyTypeComboBox.DisplayMemberPath = "TypeName";
            PropertyTypeComboBox.SelectedValuePath = "PropertyTypeID";

            PropertyStatusComboBox.ItemsSource = propertyStatuses;
            PropertyStatusComboBox.DisplayMemberPath = "StatusName";
            PropertyStatusComboBox.SelectedValuePath = "PropertyStatusID";

            FlightsList.ItemsSource = allProperties; 
        }

        private List<PropertyType> LoadPropertyTypes()
        {
            List<PropertyType> propertyTypes = new List<PropertyType>();
            string connectionString = "Server=DESKTOP-LSOM170\\SQLEXPRESS;Database=RieltorskoeAgentsvo;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PropertyTypeID, TypeName FROM PropertyTypes";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            propertyTypes.Add(new PropertyType
                            {
                                PropertyTypeID = (int)reader["PropertyTypeID"],
                                TypeName = (string)reader["TypeName"]
                            });
                        }
                    }
                }
            }
            return propertyTypes;
        }

        private List<PropertyStatus> LoadPropertyStatuses()
        {
            List<PropertyStatus> propertyStatuses = new List<PropertyStatus>();
            string connectionString = "Server=DESKTOP-LSOM170\\SQLEXPRESS;Database=RieltorskoeAgentsvo;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PropertyStatusID, StatusName FROM PropertyStatuses";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            propertyStatuses.Add(new PropertyStatus
                            {
                                PropertyStatusID = (int)reader["PropertyStatusID"],
                                StatusName = (string)reader["StatusName"]
                            });
                        }
                    }
                }
            }
            return propertyStatuses;
        }

        public class PropertyType
        {
            public int PropertyTypeID { get; set; }
            public string TypeName { get; set; }
        }

        public class PropertyStatus
        {
            public int PropertyStatusID { get; set; }
            public string StatusName { get; set; }
        }



        private void FlightsList_Loaded_1(object sender, RoutedEventArgs e)
        {
            var airports = Class1.dbconnect.Propertiezzz.ToList();

            if (airports != null && airports.Count > 0)
            {
                var flightList = airports.Select(flight => new Propertiezzz
                {
                    Address = flight.Address,
                    PropertyType = flight.PropertyType,
                    Price = flight.Price != null ? Convert.ToInt32(flight.Price) : 0,
                    Size = flight.Size != null ? Convert.ToInt32(flight.Size) : 0,
                    Bedrooms = flight.Bedrooms != null ? Convert.ToInt32(flight.Bedrooms) : 0,
                    Description = flight.Description,
                    Bathrooms = flight.Bathrooms != null ? Convert.ToInt32(flight.Bathrooms) : 0,
                    PropertyStatusID = flight.PropertyStatusID,
                    PropertyTypeID = flight.PropertyTypeID
                }).ToList();

                FlightsList.ItemsSource = flightList;
            }
            else
            {
                FlightsList.ItemsSource = null;
            }
        }

        private void filterReset_Click_1(object sender, RoutedEventArgs e)
        {
            PropertyTypeComboBox.SelectedIndex = -1;
            PropertyStatusComboBox.SelectedIndex = -1;
            allProperties = LoadAllProperties();
            FlightsList.ItemsSource = allProperties;
        }

        private void balanceTxt_Loaded(object sender, RoutedEventArgs e)
        {
            //var tempClient = Class1.dbconnect.Clients.FirstOrDefault(id => id.ClientID == c)
        }

        private ObservableCollection<Propertiezzz> properties = new ObservableCollection<Propertiezzz>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FlightsList.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите недвижимость для покупки.");
                return;
            }
            try
            {
                if (FlightsList.SelectedItem is Propertiezzz selectedProperty)
                {
                    var tempClient = Class1.dbconnect.Clients.FirstOrDefault(cli => cli.FirstName == firstname);

                    if (tempClient != null && tempClient.Balance >= selectedProperty.Price)
                    {
                        tempClient.Balance -= Convert.ToInt32(selectedProperty.Price);
                        Class1.dbconnect.SaveChanges();

                        var listViewItem = new ListViewItem
                        {
                            Content = $"Куплено: {selectedProperty.Address}, " +
                                      $"Цена: {selectedProperty.Price}, " +
                                      $"Вид: {selectedProperty.PropertyType}, " +
                                      $"Размер: {selectedProperty.Size}, " +
                                      $"Кол-во спален: {selectedProperty.Bedrooms}, " +
                                      $"Кол-во ванных: {selectedProperty.Bathrooms}, " +
                                      $"Описание: {selectedProperty.Description}, " +
                                      $"Код статуса: {selectedProperty.PropertyStatusID}, " +
                                      $"Код вида: {selectedProperty.PropertyTypeID}"
                        };

                        transactionsListView.Items.Add(listViewItem);

                        var tempTrans = new Transactions
                        {
                            ClientID = tempClient.ClientID,
                            TransactionAmount = Convert.ToInt32((decimal)tempClient.Balance) - (Convert.ToInt32((decimal)tempClient.Balance) - Convert.ToInt32(selectedProperty.Price)),
                            TransactionType = "Покупка"
                        };


                        Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;
                        var propertyToDelete = Class1.dbconnect.Propertiezzz.FirstOrDefault(a => a.Address == selectedProp.Address);
                        Class1.dbconnect.Propertiezzz.Remove(propertyToDelete);
                        Class1.dbconnect.SaveChanges();
                        FlightsList_Loaded_1(null, null);
                        Class1.dbconnect.Transactions.Add(tempTrans);
                        Class1.dbconnect.SaveChanges();

                        MessageBox.Show($"Вы успешно купили недвижимость по адресу: {selectedProperty.Address}!");
                        balanceTxt.Text = Convert.ToString(tempClient.Balance);
                    }
                    else if (tempClient == null)
                    {
                        MessageBox.Show("Клиент не найден.");
                    }
                    else
                    {
                        MessageBox.Show("Недостаточно средств для покупки этой недвижимости.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите корректную недвижимость для покупки.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при покупке: {ex.Message}");
            }
        }
        private void sell_Click(object sender, RoutedEventArgs e)
        {
            if (transactionsListView.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите недвижимость для продажи.");
                return;
            }

            try
            {
                var selectedTransaction = transactionsListView.SelectedItem as ListViewItem;

                // Вывод содержимого для отладки
                string content = selectedTransaction.Content.ToString();
                MessageBox.Show($"Содержимое транзакции: {content}");

                // Извлечение деталей транзакции
                string[] transactionDetails = content.Split(new[] { ", " }, StringSplitOptions.None);

                // Проверка длины массива перед доступом к элементам
                if (transactionDetails.Length < 9)
                {
                    MessageBox.Show("Неверный формат данных транзакции. Данные содержат " + transactionDetails.Length + " элементов.");
                    return;
                }

                string address = transactionDetails[0].Replace("Куплено: ", "");
                decimal price = decimal.Parse(transactionDetails[1].Replace("Цена: ", ""));
                string propertyType = transactionDetails[2].Replace("Вид: ", "");
                int size = int.Parse(transactionDetails[3].Replace("Размер: ", ""));
                int bedrooms = int.Parse(transactionDetails[4].Replace("Кол-во спален: ", ""));
                int bathrooms = int.Parse(transactionDetails[5].Replace("Кол-во ванных: ", ""));
                string description = transactionDetails[6].Replace("Описание: ", "");
                int statusCode = int.Parse(transactionDetails[7].Replace("Код статуса: ", ""));
                int typeCode = int.Parse(transactionDetails[8].Replace("Код вида: ", ""));

                var tempClient = Class1.dbconnect.Clients.FirstOrDefault(cli => cli.FirstName == firstname);

                if (tempClient != null)
                {
                    // Увеличиваем баланс клиента
                    tempClient.Balance += Convert.ToInt32(price);
                    Class1.dbconnect.SaveChanges();

                    // Создаем новую запись о продаже
                    var tempTrans = new Transactions
                    {
                        ClientID = tempClient.ClientID,
                        TransactionAmount = Convert.ToInt32(price),
                        TransactionType = "Продажа"
                    };
                    Class1.dbconnect.Transactions.Add(tempTrans);

                    // Удаляем элемент из списка транзакций
                    transactionsListView.Items.Remove(selectedTransaction);

                    // Удаляем недвижимость из базы данных (если это необходимо)
                    var propertyToDelete = Class1.dbconnect.Propertiezzz.FirstOrDefault(p => p.Address == address && p.Price == price);
                    if (propertyToDelete != null)
                    {
                        Class1.dbconnect.Propertiezzz.Remove(propertyToDelete);
                        Class1.dbconnect.SaveChanges();
                    }

                    var propertyToSell = new Propertiezzz
                    {
                        Address = address,
                        Price = price,
                        PropertyType = propertyType,
                        Size = size,
                        Bedrooms = bedrooms,
                        Bathrooms = bathrooms,
                        Description = description,
                        PropertyStatusID = statusCode,
                        PropertyTypeID = typeCode
                    };
                    Class1.dbconnect.Propertiezzz.Add(propertyToSell);
                    Class1.dbconnect.SaveChanges();

                    FlightsList_Loaded_1(null, null);

                    MessageBox.Show($"Вы успешно продали недвижимость по адресу: {address}!");
                    balanceTxt.Text = Convert.ToString(tempClient.Balance);
                }
                else
                {
                    MessageBox.Show("Клиент не найден.");
                }
            }
            catch (FormatException fe)
            {
                MessageBox.Show($"Ошибка формата данных: {fe.Message}");
            }
            catch (IndexOutOfRangeException ioe)
            {
                MessageBox.Show($"Ошибка доступа к массиву: {ioe.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при продаже: {ex.Message}");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.GoBack();
        }
    }
}
