using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.ViewModels.SupplierProducts
{
    public class FilterViewModel
    {
        public string SelectedSupplierName { get; set; }
        public FilterViewModel(string selectedSupplierName)
        {
            SelectedSupplierName = selectedSupplierName;
        }
    }
}
