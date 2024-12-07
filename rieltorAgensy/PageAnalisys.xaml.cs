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
    /// Логика взаимодействия для PageAnalisys.xaml
    /// </summary>
    public partial class PageAnalisys : Page
    {
        public PageAnalisys()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void DealList_Loaded(object sender, RoutedEventArgs e)
        {
            var deal = Class1.dbconnect.Transactions.ToList();

            if (deal != null && deal.Count > 0)
            {
                var flightList = deal.Select(flight => new Transactions
                {
                    TransactionsID = flight.TransactionsID,
                    ClientID = flight.ClientID,
                    TransactionAmount = flight.TransactionAmount,
                    TransactionType = flight.TransactionType
                }).ToList();

                DealList.ItemsSource = flightList;
            }
            else
            {
                DealList.ItemsSource = null;
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                // Путь для сохранения файла
                string excelFilePath = "C:\\Users\\adel\\Desktop\\AnalysReportGeneral.xlsx"; // Замените на нужный путь

                // Создаем файл Excel
                using (var package = new ExcelPackage())
                {
                    // Создаем новый лист
                    var worksheet = package.Workbook.Worksheets.Add("Сделки");

                    // Заполняем заголовки
                    worksheet.Cells[1, 1].Value = "Код транзакции";
                    worksheet.Cells[1, 2].Value = "Код клиента";
                    worksheet.Cells[1, 3].Value = "Сумма транзакции";
                    worksheet.Cells[1, 4].Value = "Тип транзакции";

                    // Заполнение данными из ListView
                    for (int i = 0; i < DealList.Items.Count; i++)
                    {
                        var deal = DealList.Items[i] as Transactions; // Убедитесь, что у вас есть класс Deals
                        worksheet.Cells[i + 2, 1].Value = deal.TransactionsID;
                        worksheet.Cells[i + 2, 2].Value = deal.ClientID; // Цена
                        worksheet.Cells[i + 2, 3].Value = deal.TransactionAmount; // Статус
                        worksheet.Cells[i + 2, 4].Value = deal.TransactionType; // Код недвижимости
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
            MainWindow.Instance.Mainframe.NavigationService.GoBack();
        }
    }
}
