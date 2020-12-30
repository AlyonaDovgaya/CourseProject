using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.ViewModels.Suppliers
{
    public class FilterViewModel
    {
        public string SelectedName { get; set; }
        public int? SelectedDeliveryMethodId { get; set; }
        public int? SelectedProductTypeId { get; set; }

        public SelectList DeliveryMethods { get; set; }
        public SelectList ProductTypes { get; set; }

        public FilterViewModel(string selectedName, int? selectedDeliveryMethodId, int? selectedProductTypeId, List<Models.DeliveryMethod> deliveryMethods, List<Models.ProductType> productTypes)
        {
            SelectedName = selectedName;
            SelectedDeliveryMethodId = selectedDeliveryMethodId;
            SelectedProductTypeId = selectedProductTypeId;

            deliveryMethods.Insert(0, new Models.DeliveryMethod { Id = 0, Name = "All" });
            productTypes.Insert(0, new Models.ProductType { Id = 0, Name = "All" });

            DeliveryMethods = new SelectList(deliveryMethods, "Id", "Name", selectedDeliveryMethodId);
            ProductTypes = new SelectList(productTypes, "Id", "Name", selectedProductTypeId);
        }
    }
}
