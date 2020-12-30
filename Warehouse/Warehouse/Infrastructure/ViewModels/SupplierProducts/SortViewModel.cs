using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.ViewModels.SupplierProducts
{
    public class SortViewModel
    {
        public enum Sort
        {
            SupplierNameAsc,
            SupplierNameDesc
        }

        public Sort SupplierNameSort { get; set; }
        public Sort Current { get; set; }

        public SortViewModel(Sort sort)
        {
            SupplierNameSort = sort == Sort.SupplierNameAsc ? Sort.SupplierNameDesc : Sort.SupplierNameAsc;
            Current = sort;
        }
    }
}
