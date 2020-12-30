using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.ViewModels.CustomerProducts
{
    public class SortViewModel
    {
        public enum Sort
        {
            CustomerNameAsc,
            CustomerNameDesc
        }

        public Sort CustomerNameSort { get; set; }
        public Sort Current { get; set; }

        public SortViewModel(Sort sort)
        {
            CustomerNameSort = sort == Sort.CustomerNameAsc ? Sort.CustomerNameDesc : Sort.CustomerNameAsc;
            Current = sort;
        }
    }
}
