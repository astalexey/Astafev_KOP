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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;

namespace View
{
    public partial class GetListForm : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        SupplyLogic logicS;
        ReportLogic logicR;
        List<SupplyViewModel> list = new List<SupplyViewModel>();
        public GetListForm(SupplyLogic logicS, ReportLogic logicR)
        {
            InitializeComponent();
            this.logicS = logicS;
            this.logicR = logicR;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                if (list != null)
                {
                    DataGridView.ItemsSource = list;
                    DataGridView.Columns[0].Visibility = Visibility.Hidden;
                    DataGridView.Columns[1].Visibility = Visibility.Hidden;
                    DataGridView.Columns[2].Visibility = Visibility.Hidden;
                    DataGridView.Columns[6].Visibility = Visibility.Hidden;
                    DataGridView.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }
        private void buttonWord_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "docx|*.docx" })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        //logicR.SaveToWordFile(dialog.FileName, list);
                        System.Windows.MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                    }
                }
            }
        }

        private void buttonExcel_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        logicR.SaveToExcelFile(dialog.FileName, list);
                        System.Windows.MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                    }
                }
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {

            Close();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<SelectSupplyForm>();
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                if (!list.Contains(logicS.Read(new SupplyBindingModel { Id = window.Id })[0]))
                {
                    list.Add(logicS.Read(new SupplyBindingModel { Id = window.Id })[0]);
                    LoadData();
                }
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridView.SelectedIndex != -1)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo,
               MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SupplyViewModel supply = (SupplyViewModel)DataGridView.SelectedCells[0].Item;
                    try
                    {
                        list.Remove(supply);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                    }
                    LoadData();
                }
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
    }
}
