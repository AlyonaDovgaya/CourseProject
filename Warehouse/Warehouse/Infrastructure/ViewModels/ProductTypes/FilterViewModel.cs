using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.ViewModels.ProductTypes
{
    public class FilterViewModel
    {
        public string SelectedName { get; set; }
        public FilterViewModel(string selectedName)
        {
            SelectedName = selectedName;
        }
    }
}
