using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Infrastructure.EntityOperations
{
    public class SupplierOperations
    {
        public IQueryable<Models.Supplier> Filter(IQueryable<Models.Supplier> suppliers, string selectedName, int? selectedDeliveryMethodId, int? selectedProductTypeId, IQueryable<Models.SupplierProduct> supplierProducts, IQueryable<Models.Product> products)
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                suppliers = suppliers.Where(s => s.Name.Contains(selectedName));
            }

            if (selectedProductTypeId != null && selectedProductTypeId != 0)
            {
                var suppliersTypes = supplierProducts
                    .Join(suppliers, sp => sp.SupplierId, s => s.Id, (sp, s) => new
                    {
                        Supplier = s.Id,
                        Product = sp.ProductId
                    })
                    .Join(products, s => s.Product, p => p.Id, (s, p) => new
                    {
                        s.Supplier,
                        Type = p.ProductTypeId
                    });

                suppliers = suppliers
                    .Where(s => 
                        suppliersTypes
                        .Where(st => st.Supplier == s.Id)
                        .Select(st => st.Type)
                        .Contains(selectedProductTypeId));
            }

            if (selectedDeliveryMethodId != null && selectedDeliveryMethodId != 0)
            {
                var suppliersMethods = supplierProducts
                    .Join(suppliers, sp => sp.DeliveryMethodId, s => s.Id, (sp, s) => new
                    {
                        Supplier = s.Id,
                        Method = sp.DeliveryMethodId
                    });

                suppliers = suppliers
                    .Where(s =>
                        suppliersMethods
                        .Where(st => st.Supplier == s.Id)
                        .Select(st => st.Method)
                        .Contains(selectedDeliveryMethodId));
            }

            return suppliers;
        }

        public IQueryable<Models.Supplier> Sort(IQueryable<Models.Supplier> suppliers, ViewModels.Suppliers.SortViewModel.Sort sortState)
        {
            switch (sortState)
            {
                case ViewModels.Suppliers.SortViewModel.Sort.NameAsc:
                    suppliers = suppliers.OrderBy(p => p.Name);
                    break;
                case ViewModels.Suppliers.SortViewModel.Sort.NameDesc:
                    suppliers = suppliers.OrderByDescending(p => p.Name);
                    break;
            }
            return suppliers;
        }

        public IQueryable<Models.Supplier> Paging(IQueryable<Models.Supplier> suppliers, bool isFromFilter, int page, int pageSize)
        {
            if (isFromFilter)
            {
                page = 1;
            }
            return suppliers.Skip(((int)page - 1) * pageSize).Take(pageSize);
        }

        public void GetFilterCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilterForm, ref string selectedName, ref int? selectedDeliveryMethodId, ref int? selectedProductTypeId)
        {
            if (string.IsNullOrEmpty(selectedName))
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "supplierSelectedName", out selectedName);
                }
            }

            if (selectedProductTypeId == null || selectedProductTypeId == 0)
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "supplierSelectedProductType", out string selectedTypeIdStr);
                    selectedProductTypeId = string.IsNullOrEmpty(selectedTypeIdStr)
                        ? null
                        : (int?)int.Parse(selectedTypeIdStr);
                }
            }

            if (selectedDeliveryMethodId == null || selectedDeliveryMethodId == 0)
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "supplierSelectedDeliveryMethod", out string selectedDeliveryMethodIdStr);
                    selectedDeliveryMethodId = string.IsNullOrEmpty(selectedDeliveryMethodIdStr)
                        ? null
                        : (int?)int.Parse(selectedDeliveryMethodIdStr);
                }
            }
        }

        public void GetSortPagingCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilter, ref int? page, ref ViewModels.Suppliers.SortViewModel.Sort? sortState)
        {
            if (!isFromFilter)
            {
                if (page == null)
                {
                    if (cookies.TryGetValue(username + "supplierPage", out string pageStr))
                    {
                        page = int.Parse(pageStr);
                    }
                }
                if (sortState == null)
                {
                    if (cookies.TryGetValue(username + "supplierSort", out string sortStateStr))
                    {
                        sortState = (ViewModels.Suppliers.SortViewModel.Sort)Enum.Parse(typeof(ViewModels.Suppliers.SortViewModel.Sort), sortStateStr);
                    }
                }
            }
        }

        public void SetDefaultValuesIfNull(ref string selectedName, ref int? page, ref ViewModels.Suppliers.SortViewModel.Sort? sortState)
        {
            selectedName ??= "";
            page ??= 1;
            sortState ??= ViewModels.Suppliers.SortViewModel.Sort.NameAsc;
        }

        public void SetCookies(IResponseCookies cookies, string username, string selectedName, int? page, ViewModels.Suppliers.SortViewModel.Sort? sortState, int? selectedDeliveryMethodId, int? selectedProductTypeId)
        {
            cookies.Append(username + "supplierSelectedName", selectedName);
            cookies.Append(username + "supplierPage", page.ToString());
            cookies.Append(username + "supplierSort", sortState.ToString());
            cookies.Append(username + "supplierSelectedDeliveryMethod", selectedDeliveryMethodId.ToString());
            cookies.Append(username + "supplierSelectedProductType", selectedProductTypeId.ToString());
        }
    }
}
