using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.ViewModels.Manufacturers
{
    public class SortViewModel
    {
        public enum Sort
        {
            NameAsc,
            NameDesc
        }

        public Sort NameSort { get; set; }
        public Sort Current { get; set; }

        public SortViewModel(Sort sort)
        {
            NameSort = sort == Sort.NameAsc ? Sort.NameDesc : Sort.NameAsc;
            Current = sort;
        }
    }
}
