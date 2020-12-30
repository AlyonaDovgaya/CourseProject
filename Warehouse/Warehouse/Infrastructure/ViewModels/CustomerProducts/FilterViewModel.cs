using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.ViewModels.CustomerProducts
{
    public class FilterViewModel
    {
        public string SelectedCustomerName { get; set; }
        public FilterViewModel(string selectedCustomerName)
        {
            SelectedCustomerName = selectedCustomerName;
        }
    }
}
