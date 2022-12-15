using BisinessLogic.BindingModels;
using BisinessLogic.FileModels;
using BisinessLogic.Interfaces;
using BisinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly IOrderStorage orderStorage;
        private readonly ISupplyStorage supplyStorage;
        private readonly IGetTechniqueStorage getTechniqueStorage;

        public ReportLogic(ISupplyStorage
      supplyStorage, IGetTechniqueStorage getTechniqueStorage, IOrderStorage orderStorage)
        {
            this.supplyStorage = supplyStorage;
            this.getTechniqueStorage = getTechniqueStorage;
            this.orderStorage = orderStorage;
        }

        public List<ReportGetTechniqueViewModel> GetSupplyTechniques(List<SupplyViewModel> supplies)
        {
            var orders = orderStorage.GetFullList();
            var techniques = getTechniqueStorage.GetFullList();

            var list = new List<ReportGetTechniqueViewModel>();

            foreach (var supply in supplies)
            {
                foreach (var order in orders)
                {
                    if (supply.SupplyOrders.ContainsKey(order.Id))
                    {
                        foreach (var technique in techniques)
                        {
                            if (technique.SupplyGetTechnique.ContainsKey(order.Id))
                            {
                                list.Add(new ReportGetTechniqueViewModel
                                {
                                    OrderName = order.OrderName,
                                    GetTechniqueArrivalTime = technique.ArrivalTime.Value,
                                    OrderPrice = order.Price
                                });
                            }
                        }
                    }
                }
            }
            return list;
        }
        public List<ReportGetTechniqueDeliveriesViewModel> GetGetTechniqueDeliveries(ReportGetTechniqueDeliveriesBindingModel model)
        {
            var getTechniques = getTechniqueStorage.GetFilteredList(new GetTechniqueBindingModel
            {
                CustomerId = model.CustomerId,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            });
            var orders = orderStorage.GetFullList();
            var supplies = supplyStorage.GetFullList();

            var list = new List<ReportGetTechniqueDeliveriesViewModel>();

            foreach (var component in orders)
            {
                foreach (var supply in supplies)
                {
                    if (supply.SupplyOrders.ContainsKey(component.Id))
                    {
                        foreach (var getTechnique in getTechniques)
                        {
                            if (supply.SupplyOrders.ContainsKey(component.Id))
                            {
                                list.Add(new ReportGetTechniqueDeliveriesViewModel
                                {
                                    OrderName = component.OrderName,
                                    SupplyName = supply.SupplyName,
                                    GetTechniqueArriveTime = getTechnique.ArrivalTime.Value
                                });
                            }
                        }
                    }
                }
            }
            return list;
        }

        public void SaveToExcelFile(string fileName, List<SupplyViewModel> supplies)
        {
            SaveToExcel.CreateCustomerDocument(new Info
            {
                FileName = fileName,
                Title = "Список закупок по поставкам",
                GetTechniques = GetSupplyTechniques(supplies)
            });
        }
    }
}
