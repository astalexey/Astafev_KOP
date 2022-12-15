using BisinessLogic.BusinessLogics;
using BisinessLogic.Interfaces;
using DatabaseImplements.Implements;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<ICustomerStorage, CustomerStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<CustomerLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IOrderStorage, OrderStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<OrderLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ISupplyStorage, SupplyStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<SupplyLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IGetTechniqueStorage, GetTechniqueStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<GetTechniqueLogic>(new HierarchicalLifetimeManager());
            var mainWindow = currentContainer.Resolve<AuthorizationForm>();
            Application.Current.MainWindow = mainWindow;
            Application.Current.MainWindow.Show();
        }
    }
}
