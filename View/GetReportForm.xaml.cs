using System;
using System.Windows;
using Unity;
using Microsoft.Reporting.WinForms;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text.RegularExpressions;
using BisinessLogic.BusinessLogics;
using BisinessLogic.BindingModels;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для GetReportForm.xaml
    /// </summary>
    public partial class GetReportForm : Window
    {
        public new IUnityContainer Container { get; set; }
        ReportLogic _logic;
        public int _customerId { get; set; }
        public GetReportForm(ReportLogic logic)
        {
            InitializeComponent();
            _logic = logic;
        }
        private void ReportViewer_Load(object sender, EventArgs e)
        {
            reportViewer.LocalReport.ReportPath = "../../Report.rdlc";
        }
        private void btnMail_Click(object sender, RoutedEventArgs e)
        {
        }


        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            if (dpFrom.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату начала",
               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpTo.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату окончания",
               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpFrom.SelectedDate >= dpTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания",
               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var dataSource = _logic.GetGetTechniqueDeliveries(new ReportGetTechniqueDeliveriesBindingModel
                {
                    CustomerId = _customerId,
                    DateFrom = dpFrom.SelectedDate,
                    DateTo = dpTo.SelectedDate
                });
                ReportDataSource source = new ReportDataSource("DataSetReportByDate", dataSource);
                reportViewer.LocalReport.DataSources.Add(source);
                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }
    }
}
