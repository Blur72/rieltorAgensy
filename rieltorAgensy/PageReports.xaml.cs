using OfficeOpenXml;
using System;
using System.Collections.Generic;
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

namespace rieltorAgensy
{
    /// <summary>
    /// Логика взаимодействия для PageReports.xaml
    /// </summary>
    public partial class PageReports : Page
    {
        public PageReports()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Mainframe.GoBack();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                // Путь для сохранения файла
                string excelFilePath = "C:\\Users\\adel\\Desktop\\PropertyReportGeneral.xlsx"; // Замените на нужный путь

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
                    RealtorID = flight.RealtorID,
                    DealCondition = flight.DealCondition
                }).ToList();

                DealList.ItemsSource = flightList;
            }
            else
            {
                DealList.ItemsSource = null;
            }
        }
    }
}
