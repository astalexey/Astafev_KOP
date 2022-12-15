using BisinessLogic.BindingModels;
using BisinessLogic.BusinessLogics;
using BisinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;

namespace View
{
    public partial class CreateSupplyForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private int? id;
        public int CustomerId { set { customerId = value; } }
        private int? customerId;

        private readonly SupplyLogic logicS;
        private readonly OrderLogic logicO;

        private Dictionary<int, (string, int)> supplyOrders;

        public CreateSupplyForm(SupplyLogic logicS, OrderLogic logicO)
        {
            InitializeComponent();
            this.logicS = logicS;
            this.logicO = logicO;
        }

        private void CreateSupplyForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    SupplyViewModel view = logicS.Read(new SupplyBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        dpData.SelectedDate = view.Date;
                        tbName.Text = view.SupplyName;
                        tbCost.Text = view.TotalCost.ToString();
                        supplyOrders = view.SupplyOrders;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                supplyOrders = new Dictionary<int, (string, int)>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (supplyOrders != null)
                {
                    List<SuppyOrdersViewModel> list = new List<SuppyOrdersViewModel>();
                    foreach (var order in supplyOrders)
                    {
                        list.Add(new SuppyOrdersViewModel { Id = order.Key, OrderName = order.Value.Item1, OrderCount = order.Value.Item2 });
                    }
                    DataGridOrders.ItemsSource = list;
                    DataGridOrders.Columns[0].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void CalcTotalCost()
        {
            try
            {
                int totalCost = 0;
                foreach (var so in supplyOrders)
                {
                    totalCost += so.Value.Item2 * (int)logicO.Read(new OrderBindingModel { Id = so.Key })?[0].Price;
                }
                tbCost.Text = totalCost.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<SelectOrderForm>();
            window.CustomerId = (int)customerId;
            if (window.ShowDialog().Value)
            {
                if (supplyOrders.ContainsKey(window.Id))
                {
                    supplyOrders[window.Id] = (window.OrderName, window.Count);
                }
                else
                {
                    supplyOrders.Add(window.Id, (window.OrderName, window.Count));
                }
                LoadData();
                CalcTotalCost();
            }
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridOrders.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        supplyOrders.Remove(((DataGridSupplyItemViewModel)DataGridOrders.SelectedCells[0].Item).Id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (supplyOrders == null || supplyOrders.Count == 0)
            {
                MessageBox.Show("Заполните заказы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicS.CreateOrUpdate(new SupplyBindingModel
                {
                    Id = id,
                    Date = dpData.SelectedDate.Value,
                    SupplyName = tbName.Text,
                    TotalCost = Convert.ToDecimal(tbCost.Text),
                    SupplyOrders = supplyOrders,
                    CustomerId = customerId
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
