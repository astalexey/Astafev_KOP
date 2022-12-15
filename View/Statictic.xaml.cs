using BisinessLogic.BindingModels;
using BisinessLogic.BusinessLogics;
using BisinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Unity;
using LiveCharts;
using LiveCharts.Wpf;

namespace View
{
    public partial class Statictic : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        SupplyLogic _logic;

        private List<StatisticBySupplyViewModel> _supplies = new List<StatisticBySupplyViewModel>();

        public Statictic(SupplyLogic logic)
        {
            InitializeComponent();
            _logic = logic;
        }
        private void LoadData()
        {
            _supplies.Clear();
            List<SupplyViewModel> supplies = _logic.Read(new SupplyBindingModel { DateTo = dpTo.SelectedDate, DateFrom = dpFrom.SelectedDate });
            foreach (var supply in supplies)
            {
                int componentCount = 0;
                foreach (var component in supply.SupplyOrders)
                {
                    componentCount += component.Value.Item2;
                }
                _supplies.Add(new StatisticBySupplyViewModel { SupplyName = supply.SupplyName, ComponentCount = componentCount });
            }
            DataGridView.ItemsSource = _supplies;
            DataGridView.Items.Refresh();
        }

        private void BuildGraph_Click(object sender, RoutedEventArgs e)
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
            LoadData();
            Build(_supplies);
        }

        private void Build(List<StatisticBySupplyViewModel> statistic)
        {
            SeriesCollection series = new SeriesCollection();
            List<string> supplyName = new List<string>();
            ChartValues<int> componentCount = new ChartValues<int>();

            foreach (var item in statistic)
            {
                supplyName.Add(item.SupplyName);
                componentCount.Add(item.ComponentCount);
            }

            Graph.AxisX.Clear();
            Graph.AxisX.Add(new Axis()

            {
                Title = "\nПоставка",
                Labels = supplyName
            });

            LineSeries procedureLine = new LineSeries
            {
                Title = "Кол-во компонент: ",
                Values = componentCount
            };

            series.Add(procedureLine);
            Graph.Series = series;
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
        }

        public static string GetPropertyDisplayName(object descriptor)
        {
            PropertyDescriptor pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                DisplayNameAttribute displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }
            }
            else
            {
                PropertyInfo pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        DisplayNameAttribute displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }
            return null;
        }
    }
}
