using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.ViewModels.Customers
{
    public class FilterViewModel
    {
        public string SelectedName { get; set; }
        public int? SelectedMinOrderQuantity { get; set; }
        public int? SelectedMaxOrderQuantity { get; set; }
        public FilterViewModel(string selectedName, int? selectedMinOrderQuantity, int? selectedMaxOrderQuantity)
        {
            SelectedName = selectedName;
            SelectedMinOrderQuantity = selectedMinOrderQuantity;
            SelectedMaxOrderQuantity = selectedMaxOrderQuantity;
        }
    }
}
