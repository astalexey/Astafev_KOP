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
    /// <summary>
    /// Логика взаимодействия для CreateTechniqueForm.xaml
    /// </summary>
    public partial class CreateTechniqueForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }
        public int CustomerId { set { customerId = value; } }

        private int? id;

        private int? customerId;

        private Dictionary<int, string> supplyGetTechiques;

        private readonly GetTechniqueLogic logicTG;
        private readonly SupplyLogic logicS;

        public CreateTechniqueForm(SupplyLogic logicS, GetTechniqueLogic logicTG)
        {
            InitializeComponent();
            this.logicS = logicS;
            this.logicTG = logicTG;
        }

        private void CreateTechniqueForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    GetTechniqueViewModel view = logicTG.Read(new GetTechniqueBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        TextBoxIssueDate.Text = view.ArrivalTime.ToString();
                        supplyGetTechiques = view.SupplyGetTechnique;
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
                supplyGetTechiques = new Dictionary<int, string>();
            }
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                TextBoxIssueDate.Text = DateTime.Now.ToString();
                if (supplyGetTechiques != null)
                {
                    dataGrid.Columns.Clear();
                    var list = new List<SupplyGetTechiquesViewModel>();
                    foreach (var sgt in supplyGetTechiques)
                    {
                        list.Add(new SupplyGetTechiquesViewModel()
                        {
                            Id = sgt.Key,
                            SupplyName = sgt.Value
                        });
                    }
                    dataGrid.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<SelectSupplyForm>();
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                if (!supplyGetTechiques.ContainsKey(window.Id))
                {
                    supplyGetTechiques.Add(window.Id, window.SupplyName);
                }
                else
                {
                    MessageBox.Show("Данная поставка уже выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }
            LoadData();
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells.Count != 0)
            {
                var window = Container.Resolve<SelectComponentForm>();
                window.Id = ((GetTechniqueBindingModel)dataGrid.SelectedCells[0].Item).Id.Value;
                if (window.ShowDialog().Value)
                {
                    supplyGetTechiques[window.Id] = (window.ComponentName);
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        supplyGetTechiques.Remove(((GetTechniqueBindingModel)dataGrid.SelectedCells[0].Item).Id.Value);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (supplyGetTechiques == null || supplyGetTechiques.Count == 0)
            {
                MessageBox.Show("Заполните поставки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicTG.CreateOrUpdate(new GetTechniqueBindingModel
                {
                    Id = id,
                    ArrivalTime = DateTime.Now,
                    CustomerId = customerId,
                    SupplyGetTechnique = supplyGetTechiques
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

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
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
                // Check for DisplayName attribute and set the column header accordingly
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
                    // Check for DisplayName attribute and set the column header accordingly
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

