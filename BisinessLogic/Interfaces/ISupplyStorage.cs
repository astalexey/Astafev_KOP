using BisinessLogic.BindingModels;
using BisinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisinessLogic.Interfaces
{
    public interface ISupplyStorage
    {
        List<SupplyViewModel> GetFullList();
        List<SupplyViewModel> GetFilteredList(SupplyBindingModel model);
        SupplyViewModel GetElement(SupplyBindingModel model);
        void Insert(SupplyBindingModel model);
        void Update(SupplyBindingModel model);
        void Delete(SupplyBindingModel model);
    }
}
