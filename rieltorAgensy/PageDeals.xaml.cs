using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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


namespace rieltorAgensy
{
    /// <summary>
    /// Логика взаимодействия для PageDeals.xaml
    /// </summary>
    public partial class PageDeals : Page
    {

        private string connectionString = "Server=DESKTOP-LSOM170\\\\SQLEXPRESS;Database=RieltorskoeAgentsvo;Integrated Security=True;";
        public PageDeals()
        {
            InitializeComponent();
            changeCalendar.Visibility = Visibility.Hidden;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка обязательных полей
                if (!myCalendar.SelectedDate.HasValue ||
                    string.IsNullOrWhiteSpace(priceText.Text) ||
                    string.IsNullOrWhiteSpace(statusText.Text) ||
                    string.IsNullOrWhiteSpace(kodPropText.Text) ||
                    string.IsNullOrWhiteSpace(kodClientText.Text) ||
                    string.IsNullOrWhiteSpace(kodRealtorText.Text) ||
                    string.IsNullOrWhiteSpace(DealCond.Text))
                {
                    MessageBox.Show("Вы не заполнили обязательные поля!");
                    return;
                }

                DateTime selectedDate = myCalendar.SelectedDate.Value;

                // Проверка на числовые значения
                if (!int.TryParse(priceText.Text, out int price) ||
                    !int.TryParse(kodPropText.Text, out int kodProp) ||
                    !int.TryParse(kodClientText.Text, out int kodKlient) ||
                    !int.TryParse(kodRealtorText.Text, out int kodRieltor))
                {
                    MessageBox.Show("Обязательные поля должны содержать числовые значения.");
                    return;
                }

                // Проверка наличия сделки
                var deal = Class1.dbconnect.Deals.FirstOrDefault(d => d.DealDate == selectedDate);

                if (deal != null)
                {
                    MessageBox.Show("Такая сделка уже есть");
                }
                else
                {
                    try
                    {
                        // Создание новой сделки
                        var tempDeal = new Deals()
                        {
                            DealDate = selectedDate,
                            Price = price,
                            Status = statusText.Text,
                            PropertyID = kodProp,
                            ClientID = kodKlient,
                            RealtorID = kodRieltor,
                            DealCondition = DealCond.Text
                        };

                        // Добавление сделки в базу данных
                        Class1.dbconnect.Deals.Add(tempDeal);
                        Class1.dbconnect.SaveChanges();
                        MessageBox.Show("Сделка сохранена!");
                        DealList_Loaded(sender, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Произошла ошибка при сохранении сделки: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обработке: {ex.Message}");
            }
        }

        private void DealList_Loaded(object sender, RoutedEventArgs e)
        {
            var deal = Class1.dbconnect.Deals.ToList();

            if (deal != null && deal.Count > 0)
            {
                var flightList = deal.Select(flight => new Deals
                {
                    DealID = flight.DealID,
                    DealDate = flight.DealDate,
                    Price = flight.Price != null ? Convert.ToInt32(flight.Price) : 0,
                    Status = flight.Status,
                    PropertyID = flight.PropertyID,
                    ClientID = flight.ClientID,
                    RealtorID= flight.RealtorID,
                    DealCondition = flight.DealCondition
                }).ToList();

                DealList.ItemsSource = flightList;
            }
            else
            {
                DealList.ItemsSource = null;
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Deals selectedDeal = DealList.SelectedItem as Deals;

            if (selectedDeal != null)
            {
                var dealToDelete = Class1.dbconnect.Deals.FirstOrDefault(d => d.DealDate == selectedDeal.DealDate);
                if (dealToDelete != null)
                {
                    Class1.dbconnect.Deals.Remove(dealToDelete);
                    Class1.dbconnect.SaveChanges();
                    DealList_Loaded(null, null);
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
                if (DealList.SelectedItem != null)
                {
                    if (EditCB.SelectedItem is ComboBoxItem item)
                    {
                        if (item.Content.ToString() == "Дата")
                        {
                            Deals selectedDeal = DealList.SelectedItem as Deals;
                            selectedDeal.DealDate = changeCalendar.SelectedDate;
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            DealList.ItemsSource = Class1.dbconnect.Deals.ToList();
                        }
                        else if (item.Content.ToString() == "Цена")
                        {
                            Deals selectedDeal = DealList.SelectedItem as Deals;
                            selectedDeal.Price = Convert.ToInt32(Changetext.Text);
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            DealList.ItemsSource = Class1.dbconnect.Deals.ToList();
                        }
                        else if (item.Content.ToString() == "Статус")
                        {
                            if (Changetext.Text == "В процессе" || Changetext.Text == "Закончена" || Changetext.Text == "Начата")
                            {
                                Deals selectedDeal = DealList.SelectedItem as Deals;

                                var existingDeal = Class1.dbconnect.Deals.Find(selectedDeal.DealID);
                                if (existingDeal == null)
                                {
                                    MessageBox.Show($"Сделка с ID {selectedDeal.DealID} не найдена.");
                                    return;
                                }

                                selectedDeal.Status = Changetext.Text;
                                EditCB.SelectedValue = null;
                                Changetext.Text = null;

                                var not = new Notifications
                                {
                                    ClientID = selectedDeal.ClientID,
                                    DealID = selectedDeal.DealID,
                                    Date = DateTime.Now,
                                    Text = $"Новый статус: {selectedDeal.Status}"
                                };

                                Class1.dbconnect.Notifications.Add(not);
                                Class1.dbconnect.SaveChanges();
                                DealList.ItemsSource = Class1.dbconnect.Deals.ToList();
                            }
                            else
                            {
                                MessageBox.Show("Выбран неправильный статус");
                                return;
                            }
                        }
                        else if (item.Content.ToString() == "Код недвижимости")
                        {
                            Deals selectedDeal = DealList.SelectedItem as Deals;
                            selectedDeal.PropertyID = Convert.ToInt32(Changetext.Text);
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            DealList.ItemsSource = Class1.dbconnect.Deals.ToList();
                        }
                        else if (item.Content.ToString() == "Код клиента")
                        {
                            Deals selectedDeal = DealList.SelectedItem as Deals;
                            selectedDeal.ClientID = Convert.ToInt32(Changetext.Text);
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            DealList.ItemsSource = Class1.dbconnect.Deals.ToList();
                        }
                        else if (item.Content.ToString() == "Код риэлтора")
                        {
                            Deals selectedDeal = DealList.SelectedItem as Deals;
                            selectedDeal.RealtorID = Convert.ToInt32(Changetext.Text);
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            DealList.ItemsSource = Class1.dbconnect.Deals.ToList();
                        }
                        else if (item.Content.ToString() == "Условия сделки")
                        {
                            Deals selectedDeal = DealList.SelectedItem as Deals;
                            selectedDeal.DealCondition = Changetext.Text;
                            EditCB.SelectedValue = null;
                            Changetext.Text = null;
                            Class1.dbconnect.SaveChanges();
                            DealList.ItemsSource = Class1.dbconnect.Deals.ToList();
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

        private void EditCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditCB.SelectedItem is ComboBoxItem item)
            {
                if (item.Content.ToString() == "Дата")
                {
                    MessageBox.Show($"Выберите дату слева");
                    changeCalendar.Visibility = Visibility.Visible;
                    Changetext.Visibility = Visibility.Hidden;
                }
                else
                {
                    changeCalendar.Visibility = Visibility.Hidden;
                    Changetext.Visibility = Visibility.Visible;
                }
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
                    worksheet.Cells[1, 1].Value = "Дата";
                    worksheet.Cells[1, 2].Value = "Цена";
                    worksheet.Cells[1, 3].Value = "Статус";
                    worksheet.Cells[1, 4].Value = "Код недвижимости";
                    worksheet.Cells[1, 5].Value = "Код клиента";
                    worksheet.Cells[1, 6].Value = "Код риэлтора";
                    worksheet.Cells[1, 7].Value = "Условия сделки";

                    // Заполнение данными из ListView
                    for (int i = 0; i < DealList.Items.Count; i++)
                    {
                        var deal = DealList.Items[i] as Deals; // Убедитесь, что у вас есть класс Deals
                        worksheet.Cells[i + 2, 1].Value = deal.DealDate.HasValue ? deal.DealDate.Value.ToString("dd.MM.yyyy") : "Нет данных"; // Дата
                        worksheet.Cells[i + 2, 2].Value = deal.Price; // Цена
                        worksheet.Cells[i + 2, 3].Value = deal.Status; // Статус
                        worksheet.Cells[i + 2, 4].Value = deal.PropertyID; // Код недвижимости
                        worksheet.Cells[i + 2, 5].Value = deal.ClientID; // Код клиента
                        worksheet.Cells[i + 2, 6].Value = deal.RealtorID; // Код риэлтора
                        worksheet.Cells[i + 2, 7].Value = deal.DealCondition; // Условия сделки
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