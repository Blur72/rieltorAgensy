using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MimeKit;
using UtfUnknown.Core.Analyzers.Japanese;

namespace rieltorAgensy
{
    /// <summary>
    /// Логика взаимодействия для PageProperty.xaml
    /// </summary>
    /// 
    public partial class PageProperty : Page
    {
        int clientid;

        public PageProperty(int clientid)
        {
            InitializeComponent();
            LoadData();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            this.clientid = clientid;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string address = adres.Text;
                string propType = TypeProp.Text;
                int pr = Convert.ToInt32(priceText.Text);
                int propsize = Convert.ToInt32(area.Text);
                int bed = Convert.ToInt32(numBed.Text);
                int bath = Convert.ToInt32(numBath.Text);
                string desc = descText.Text;
                int psId = Convert.ToInt32(idStatus.Text);
                int ptId = Convert.ToInt32(idType.Text);

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
                        Price = Convert.ToDecimal(priceText.Text),
                        Size = Convert.ToDecimal(area.Text),
                        Bedrooms = Convert.ToInt32(numBed.Text),
                        Bathrooms = Convert.ToInt32(numBath.Text),
                        Description = descText.Text,
                        PropertyStatusID = Convert.ToInt32(idStatus.Text),
                        PropertyTypeID = Convert.ToInt32(idType.Text)
                    };
                    Class1.dbconnect.Propertiezzz.Add(tempProperty);
                    Class1.dbconnect.SaveChanges();
                    MessageBox.Show("Недвижимость сохранена!");

                    SendMessageEmail(tempProperty.PropertyID);
                    MessageBox.Show("На почту отправлено сообщение");
                    FlightsList_Loaded(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавление недвижимости: {ex.Message}");
            }
        }

        private void SendMessageEmail(int idProperty)
        {
            var sjdsa = Class1.dbconnect.Clients.FirstOrDefault(sad => sad.ClientID == clientid);
            var kirieshki = Class1.dbconnect.Propertiezzz.FirstOrDefault(sads => sads.PropertyID == idProperty);
            var message = new MimeMessage
            {
                From = { new MailboxAddress("АВЕНЮ", "Adel.shaihutdinov123@gmail.com") },
                To = { new MailboxAddress((sjdsa.LastName + " " + sjdsa.FirstName), $"{sjdsa.Email}") },
                Subject = ""
            };


            // Формируем тело сообщения
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 20px;
                    background-color: #f4f4f4;
                    color: #333;
                }}
                h1 {{
                    color: #ffd700;
                    text-align: center;
                    border-bottom: 2px solid #ffd700;
                    padding-bottom: 10px;
                }}
                .container {{
                    background-color: #fff;
                    padding: 20px;
                    border-radius: 5px;
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                    max-width: 600px;
                    margin: auto;
                }}
                .footer {{
                    margin-top: 20px;
                    font-size: 12px;
                    color: #777;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Добавлена недвижимость: {kirieshki.PropertyType}</h1>
            </div>
            <div class='footer'>
                <p>Спасибо за использование нашего сервиса!</p>
            </div>
        </body>
        </html>"
            };
            message.Body = bodyBuilder.ToMessageBody();

            // Отправляем сообщение через SMTP-клиент
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    // Подключаемся к SMTP серверу
                    client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    client.Authenticate("Adel.shaihutdinov123@gmail.com", "feok qbno bodp xrne"); // Вставьте сюда пароль приложения

                    // Отправляем сообщение
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка отправки сообщения: {ex.Message}");
                }
                finally
                {
                    // Отключаемся от сервера
                    client.Disconnect(true);
                }
            }
        }

        private string connectionString = "Server=DESKTOP-LSOM170\\SQLEXPRESS;Database=RieltorskoeAgentsvo;Integrated Security=True;";
        private void FlightsList_Loaded(object sender, RoutedEventArgs e)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;

            if (selectedProp != null)
            {
                var propertyToDelete = Class1.dbconnect.Propertiezzz.FirstOrDefault(a => a.Address == selectedProp.Address);
                if (propertyToDelete != null)
                {
                    Class1.dbconnect.Propertiezzz.Remove(propertyToDelete);
                    Class1.dbconnect.SaveChanges();
                    FlightsList_Loaded(null, null);
                    MessageBox.Show("Недвижимость успешно удалена.");
                }
                else
                {
                    MessageBox.Show("Недвижимость не найдена в базе данных.");
                }
            }
            else
            {
                MessageBox.Show("Недвижимость для удаления не была выбрана.");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FlightsList.SelectedItem != null)
                {
                    if (EditCB.SelectedItem is ComboBoxItem item)
                    {
                        if (item.Content.ToString() == "Адрес")
                        {
                            Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;
                            selectedProp.Address = Changetext.Text;
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            FlightsList.ItemsSource = Class1.dbconnect.Propertiezzz.ToList();
                        }
                        else if (item.Content.ToString() == "Вид недвижимости")
                        {
                            if (Changetext.Text != "Дом" || Changetext.Text != "Квартира" || Changetext.Text != "Коммерческая недвижимость")
                            {
                                Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;
                                selectedProp.PropertyType = Changetext.Text;
                                EditCB.SelectedValue = null;
                                Changetext.Text = null;
                                Class1.dbconnect.SaveChanges();
                                FlightsList.ItemsSource = Class1.dbconnect.Propertiezzz.ToList();
                            }
                            else
                            {
                                MessageBox.Show("Выбран неправильный тип");
                                return;
                            }
                        }
                        else if (item.Content.ToString() == "Статус недвижимости")
                        {
                            if (Changetext.Text != "В процессе" || Changetext.Text != "Закончена" || Changetext.Text != "Начата")
                            {
                                Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;
                                selectedProp.Status = Changetext.Text;
                                EditCB.SelectedValue = null;
                                Changetext.Text = null;
                                Class1.dbconnect.SaveChanges();
                                FlightsList.ItemsSource = Class1.dbconnect.Propertiezzz.ToList();
                                //
                            }
                            else
                            {
                                MessageBox.Show("Выбран неправильный тип");
                                return;
                            }
                        }
                        else if (item.Content.ToString() == "Цена")
                        {
                            Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;
                            selectedProp.Price = Convert.ToInt32(Changetext.Text);
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            FlightsList.ItemsSource = Class1.dbconnect.Propertiezzz.ToList();
                        }
                        else if (item.Content.ToString() == "Размер")
                        {
                            Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;
                            selectedProp.Size = Convert.ToInt32(Changetext.Text);
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            FlightsList.ItemsSource = Class1.dbconnect.Propertiezzz.ToList();
                        }
                        else if (item.Content.ToString() == "Кол-во спален")
                        {
                            Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;
                            selectedProp.Bedrooms = Convert.ToInt32(Changetext.Text);
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            FlightsList.ItemsSource = Class1.dbconnect.Propertiezzz.ToList();
                        }
                        else if (item.Content.ToString() == "Кол-во ванных")
                        {
                            Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;
                            selectedProp.Bathrooms = Convert.ToInt32(Changetext.Text);
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            FlightsList.ItemsSource = Class1.dbconnect.Propertiezzz.ToList();
                        }
                        else if (item.Content.ToString() == "Описание")
                        {
                            Propertiezzz selectedProp = FlightsList.SelectedItem as Propertiezzz;
                            selectedProp.Description = Changetext.Text;
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            FlightsList.ItemsSource = Class1.dbconnect.Propertiezzz.ToList();
                        }
                    }
                    else { MessageBox.Show("Не выбрано что менять"); }
                }
                else
                {
                    MessageBox.Show("Ничего не выбрано");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при экспорте: {ex.Message}");
            }
        }
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

            FlightsList.ItemsSource = allProperties; // Устанавливаем все объекты недвижимости в ListView
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


        private void filterReset_Click(object sender, RoutedEventArgs e)
        {
            PropertyTypeComboBox.SelectedIndex = -1;
            PropertyStatusComboBox.SelectedIndex = -1;
            allProperties = LoadAllProperties();
            FlightsList.ItemsSource = allProperties; 
        }

        private void TypeProp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TypeProp.Text != "Дом" || TypeProp.Text != "Квартира" || TypeProp.Text != "Коммерческая недвижимость")
            {
                if (TypeProp.Text == "Дом")
                {
                    idType.Text = "2";
                }
                else if (TypeProp.Text == "Квартира")
                {
                    idType.Text = "1";
                }
                else if (TypeProp.Text == "Коммерческая недвижимость")
                {
                    idType.Text = "3";
                }
            }
            else
            {
                MessageBox.Show("Вы ввели неправильный тип недвижимости");
                return;
            }
        }

        private void Status_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Status.Text != "В процессе" || Status.Text != "Выставлено" || Status.Text != "Продано")
            {
                if (Status.Text == "В процессе")
                {
                    idStatus.Text = "3";
                }
                else if (Status.Text == "Выставлено")
                {
                    idStatus.Text = "2";
                }
                else if (Status.Text == "Продано")
                {
                    idStatus.Text = "1";
                }
            }
            else
            {
                MessageBox.Show("Вы ввели неправильный статус недвижимости");
                return;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                // Путь для сохранения файла
                string excelFilePath = "C:\\Users\\adel\\Desktop\\PropertyReport.xlsx"; // Замените на нужный путь

                // Создаем файл Excel
                using (var package = new ExcelPackage())
                {
                    // Создаем новый лист
                    var worksheet = package.Workbook.Worksheets.Add("Сделки");

                    // Заполняем заголовки
                    worksheet.Cells[1, 1].Value = "Адрес";
                    worksheet.Cells[1, 2].Value = "Вид недвижимости";
                    worksheet.Cells[1, 3].Value = "Цена";
                    worksheet.Cells[1, 4].Value = "Размер";
                    worksheet.Cells[1, 5].Value = "Кол-во спален";
                    worksheet.Cells[1, 6].Value = "Кол-во ванных";
                    worksheet.Cells[1, 7].Value = "Описание";
                    worksheet.Cells[1, 8].Value = "Код статуса";
                    worksheet.Cells[1, 9].Value = "Код вида";

                    // Заполнение данными из ListView
                    for (int i = 0; i < FlightsList.Items.Count; i++)
                    {
                        var deal = FlightsList.Items[i] as Propertiezzz; // Убедитесь, что у вас есть класс Deals
                        worksheet.Cells[i + 2, 1].Value = deal.Address; // Адрес
                        worksheet.Cells[i + 2, 2].Value = deal.PropertyType; // Вид недвижимости
                        worksheet.Cells[i + 2, 3].Value = deal.Price; // Цена
                        worksheet.Cells[i + 2, 4].Value = deal.Size; // Размер
                        worksheet.Cells[i + 2, 5].Value = deal.Bedrooms; // Кол-во спален
                        worksheet.Cells[i + 2, 6].Value = deal.Bathrooms; // Кол-во ванных
                        worksheet.Cells[i + 2, 7].Value = deal.Description; // Описание
                        worksheet.Cells[i + 2, 8].Value = deal.PropertyStatusID; // Код статуса
                        worksheet.Cells[i + 2, 9].Value = deal.PropertyTypeID; // Код вида
                    }

                    // Сохранение файла
                    FileInfo excelFile = new FileInfo(excelFilePath);
                    package.SaveAs(excelFile);
                }

                // Уведомление об успешном экспорте
                MessageBox.Show("Документ успешно экспортирован в Excel!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при экспорте: {ex.Message}");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.GoBack();
        }

    }
}

