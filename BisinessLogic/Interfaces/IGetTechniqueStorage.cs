using BisinessLogic.BindingModels;
using BisinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisinessLogic.Interfaces
{
    public interface IGetTechniqueStorage
    {
        List<GetTechniqueViewModel> GetFullList();
        List<GetTechniqueViewModel> GetFilteredList(GetTechniqueBindingModel model);
        GetTechniqueViewModel GetElement(GetTechniqueBindingModel model);
        void Insert(GetTechniqueBindingModel model);
        void Update(GetTechniqueBindingModel model);
        void Delete(GetTechniqueBindingModel model);
    }
}
