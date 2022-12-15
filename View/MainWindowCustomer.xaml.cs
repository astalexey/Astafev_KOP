using BisinessLogic.BindingModels;
using BisinessLogic.BusinessLogics;
using System.Windows;
using Unity;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для MainWindowCustomer.xaml
    /// </summary>
    public partial class MainWindowCustomer : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private int? id;
        private CustomerLogic logic;
        public MainWindowCustomer(CustomerLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void MainWindowCustomer_Loaded(object sender, RoutedEventArgs e)
        {
            var client = logic.Read(new CustomerBindingModel { Id = id })?[0];
            labelCustomer.Content = "Клиент: " + client.CustomerName + " " + client.CustomerSurname;
        }
        //Заказы
        private void Order_Button(object sender, RoutedEventArgs e)
        {
            OrderForm form = Container.Resolve<OrderForm>();
            form.Id = (int)id;
            form.ShowDialog();
        }
        //Поставки
        private void Supply_Button(object sender, RoutedEventArgs e)
        {
            SupplyForm form = Container.Resolve<SupplyForm>();
            form.Id = (int)id;
            form.ShowDialog();
        }
        //Техника
        private void Technique_Button(object sender, RoutedEventArgs e)
        {
            TechniqueForm form = Container.Resolve<TechniqueForm>();
            form.Id = (int)id;
            form.ShowDialog();
        }
        //Получение списка
        private void GetList_Button(object sender, RoutedEventArgs e)
        {
            GetListForm form = Container.Resolve<GetListForm>();
            form.ShowDialog();
        }
        //График
        private void Statictic_Button(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<Statictic>();
            form.ShowDialog();
        }
    }
}
