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
    /// Логика взаимодействия для SelectComponentForm.xaml
    /// </summary>
    public partial class SelectComponentForm : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        SupplyLogic _logic;
        private SupplyViewModel componentViewModel;
        public int Id
        {
            get
            {
                return componentViewModel.Id;
            }
            set
            {
                ComboBoxComponentName.SelectedItem = value;
            }
        }
        public string ComponentName { get { return ComboBoxComponentName.Text; } }

        public int ComponentCount
        {
            get { return Convert.ToInt32(TextBoxCount.Text); }
            set
            {
                TextBoxCount.Text = value.ToString();
            }
        }

        public SelectComponentForm(SupplyLogic logic)
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
                    ComboBoxComponentName.DisplayMemberPath = "SupplyName";
                    ComboBoxComponentName.ItemsSource = list;
                    ComboBoxComponentName.SelectedItem = null;
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
                if (ComboBoxComponentName.SelectedValue == null)
                {
                    MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                    return;
                }
                if (TextBoxCount.Text == null)
                {
                    MessageBox.Show("Введите количество компонент", "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                    return;
                }
                componentViewModel = (SupplyViewModel)ComboBoxComponentName.SelectionBoxItem;
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

