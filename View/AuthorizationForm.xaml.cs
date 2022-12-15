using BisinessLogic.BindingModels;
using BisinessLogic.BusinessLogics;
using DatabaseImplements;
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
    /// Логика взаимодействия для AuthorizationForm.xaml
    /// </summary>
    public partial class AuthorizationForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly CustomerLogic logic;

        public AuthorizationForm(CustomerLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            loginBox.Text = "89999999999";
            passwordBox.Password = "123";
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginBox.Text))
            {
                MessageBox.Show("Заполните логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                MessageBox.Show("Заполните пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var view = logic.Read(new CustomerBindingModel
                {
                    Telephone = loginBox.Text,
                    Password = passwordBox.Password
                });
                if (view != null && view.Count > 0)
                {
                    //DialogResult = true;
                    var window = Container.Resolve<MainWindowCustomer>();
                    window.Id = view[0].Id;
                    window.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Неверный логин/пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Вывод пользователей в консоль (ПОТОМ ЭТО УДАЛИТЬ)
        private void OutputUsers()
        {
            Database db = new Database();

            var usersC = db.Customers.ToList();
            Console.WriteLine("Заказчики:");
            foreach (var user in usersC)
            {
                Console.WriteLine($"{user.Id}|{user.CustomerName}|{user.Patronymic}|" +
                    $"{user.CustomerSurname}|{user.Telephone}|{user.Email}|{user.Password}");
            }
        }
        // Обработка кнопки перехода на форму регистрации
        private void Regis_Click(object sender, RoutedEventArgs e)
        {
            OutputUsers();
            var form = Container.Resolve<RegistrationForm>();
            Close();
            form.ShowDialog();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
