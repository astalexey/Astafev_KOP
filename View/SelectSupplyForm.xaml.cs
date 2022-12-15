using BisinessLogic.BusinessLogics;
using BisinessLogic.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Unity;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для SelectSupplyForm.xaml
    /// </summary>
    public partial class SelectSupplyForm : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        SupplyLogic _logic;
        private SupplyViewModel supplyViewModel;
        public int Id
        {
            get
            {
                return supplyViewModel.Id;
            }
            set
            {
                ComboBoxSupplyName.SelectedItem = value;
            }
        }
        public string SupplyName { get { return ComboBoxSupplyName.Text; } }

        public SelectSupplyForm(SupplyLogic logic)
        {
            InitializeComponent();
            _logic = logic;
        }
        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            var list = _logic.Read(null);
            if (list.Count > 0)
            {
                try
                {
                    ComboBoxSupplyName.DisplayMemberPath = "SupplyName";
                    ComboBoxSupplyName.ItemsSource = list;
                    ComboBoxSupplyName.SelectedItem = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                }
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (ComboBoxSupplyName.SelectedValue == null)
                {
                    MessageBox.Show("Выберите поставку", "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                    return;
                }

                supplyViewModel = (SupplyViewModel)ComboBoxSupplyName.SelectionBoxItem;
                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }
    }
}
