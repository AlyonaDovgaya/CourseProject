using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.ViewModels.SupplierProducts
{
    public class IndexViewModel
    {
        public IEnumerable<Models.SupplierProduct> SupplierProducts { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
