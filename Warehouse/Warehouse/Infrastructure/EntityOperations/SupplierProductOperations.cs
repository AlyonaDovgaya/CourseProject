using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Infrastructure.EntityOperations
{
    public class SupplierProductOperations
    {
        public IQueryable<Models.SupplierProduct> Filter(IQueryable<Models.SupplierProduct> supplierProducts, string selectedSupplierName)
        {
            if (!string.IsNullOrEmpty(selectedSupplierName))
            {
                supplierProducts = supplierProducts.Where(p => p.Supplier.Name.Contains(selectedSupplierName));
            }
            return supplierProducts;
        }

        public IQueryable<Models.SupplierProduct> Sort(IQueryable<Models.SupplierProduct> supplierProducts, ViewModels.SupplierProducts.SortViewModel.Sort sortState)
        {
            switch (sortState)
            {
                case ViewModels.SupplierProducts.SortViewModel.Sort.SupplierNameAsc:
                    supplierProducts = supplierProducts.OrderBy(p => p.Supplier.Name);
                    break;
                case ViewModels.SupplierProducts.SortViewModel.Sort.SupplierNameDesc:
                    supplierProducts = supplierProducts.OrderByDescending(p => p.Supplier.Name);
                    break;
            }
            return supplierProducts;
        }

        public IQueryable<Models.SupplierProduct> Paging(IQueryable<Models.SupplierProduct> supplierProducts, bool isFromFilter, int page, int pageSize)
        {
            if (isFromFilter)
            {
                page = 1;
            }
            return supplierProducts.Skip(((int)page - 1) * pageSize).Take(pageSize);
        }

        public void GetFilterCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilterForm, ref string selectedSupplierName)
        {
            if (string.IsNullOrEmpty(selectedSupplierName))
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "supplierProductSelectedSupplierName", out selectedSupplierName);
                }
            }
        }

        public void GetSortPagingCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilter, ref int? page, ref ViewModels.SupplierProducts.SortViewModel.Sort? sortState)
        {
            if (!isFromFilter)
            {
                if (page == null)
                {
                    if (cookies.TryGetValue(username + "supplierProductPage", out string pageStr))
                    {
                        page = int.Parse(pageStr);
                    }
                }
                if (sortState == null)
                {
                    if (cookies.TryGetValue(username + "supplierProductSort", out string sortStateStr))
                    {
                        sortState = (ViewModels.SupplierProducts.SortViewModel.Sort)Enum.Parse(typeof(ViewModels.SupplierProducts.SortViewModel.Sort), sortStateStr);
                    }
                }
            }
        }

        public void SetDefaultValuesIfNull(ref string selectedSupplierName, ref int? page, ref ViewModels.SupplierProducts.SortViewModel.Sort? sortState)
        {
            selectedSupplierName ??= "";
            page ??= 1;
            sortState ??= ViewModels.SupplierProducts.SortViewModel.Sort.SupplierNameAsc;
        }

        public void SetCookies(IResponseCookies cookies, string username, string selectedSupplierName, int? page, ViewModels.SupplierProducts.SortViewModel.Sort? sortState)
        {
            cookies.Append(username + "supplierProductSelectedSupplierName", selectedSupplierName);
            cookies.Append(username + "supplierProductPage", page.ToString());
            cookies.Append(username + "supplierProductSort", sortState.ToString());
        }
    }
}
