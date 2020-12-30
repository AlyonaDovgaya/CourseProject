using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Infrastructure.EntityOperations
{
    public class ProductOperations
    {
        public IQueryable<Models.Product> Filter(IQueryable<Models.Product> products, string selectedName, int? selectedMinPrice, int? selectedMaxPrice, int? selectedTypeId, int? selectedManufacturerId)
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                products = products.Where(p => p.Name.Contains(selectedName));
            }

            if (selectedMinPrice != null)
            {
                products = products.Where(p => p.Price >= selectedMinPrice);
            }

            if (selectedMaxPrice != null)
            {
                products = products.Where(p => p.Price <= selectedMaxPrice);
            }

            if (selectedTypeId != null && selectedTypeId != 0)
            {
                products = products.Where(p => p.ProductTypeId == selectedTypeId);
            }

            if (selectedManufacturerId != null && selectedManufacturerId != 0)
            {
                products = products.Where(p => p.ManufacturerId == selectedManufacturerId);
            }

            return products;
        }

        public IQueryable<Models.Product> Sort(IQueryable<Models.Product> products, ViewModels.Products.SortViewModel.Sort sortState)
        {
            switch (sortState)
            {
                case ViewModels.Products.SortViewModel.Sort.NameAsc:
                    products = products.OrderBy(p => p.Name);
                    break;
                case ViewModels.Products.SortViewModel.Sort.NameDesc:
                    products = products.OrderByDescending(p => p.Name);
                    break;
            }
            return products;
        }

        public IQueryable<Models.Product> Paging(IQueryable<Models.Product> products, bool isFromFilter, int page, int pageSize)
        {
            if (isFromFilter)
            {
                page = 1;
            }
            return products.Skip(((int)page - 1) * pageSize).Take(pageSize);
        }

        public void GetFilterCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilterForm, ref string selectedName, ref int? selectedMinPrice, ref int? selectedMaxPrice, ref int? selectedTypeId, ref int? selectedManufacturerId)
        {
            if (string.IsNullOrEmpty(selectedName))
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "productSelectedName", out selectedName);
                }
            }

            if (selectedMinPrice == null)
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "productSelectedMinPrice", out string selectedMinPriceStr);
                    selectedMinPrice = string.IsNullOrEmpty(selectedMinPriceStr)
                        ? null
                        : (int?) int.Parse(selectedMinPriceStr);
                }
            }

            if (selectedMaxPrice == null)
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "productSelectedMaxPrice", out string selectedMaxPriceStr);
                    selectedMaxPrice = string.IsNullOrEmpty(selectedMaxPriceStr)
                        ? null
                        : (int?)int.Parse(selectedMaxPriceStr);
                }
            }

            if (selectedTypeId == null || selectedTypeId == 0)
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "productSelectedType", out string selectedTypeIdStr);
                    selectedTypeId = string.IsNullOrEmpty(selectedTypeIdStr)
                        ? null
                        : (int?)int.Parse(selectedTypeIdStr);
                }
            }

            if (selectedManufacturerId == null || selectedManufacturerId == 0)
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "productSelectedManufacturer", out string selectedManufacturerIdStr);
                    selectedManufacturerId = string.IsNullOrEmpty(selectedManufacturerIdStr)
                        ? null
                        : (int?)int.Parse(selectedManufacturerIdStr);
                }
            }
        }

        public void GetSortPagingCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilter, ref int? page, ref ViewModels.Products.SortViewModel.Sort? sortState)
        {
            if (!isFromFilter)
            {
                if (page == null)
                {
                    if (cookies.TryGetValue(username + "productPage", out string pageStr))
                    {
                        page = int.Parse(pageStr);
                    }
                }
                if (sortState == null)
                {
                    if (cookies.TryGetValue(username + "productSort", out string sortStateStr))
                    {
                        sortState = (ViewModels.Products.SortViewModel.Sort)Enum.Parse(typeof(ViewModels.Products.SortViewModel.Sort), sortStateStr);
                    }
                }
            }
        }

        public void SetDefaultValuesIfNull(ref string selectedName, ref int? page, ref ViewModels.Products.SortViewModel.Sort? sortState)
        {
            selectedName ??= "";
            page ??= 1;
            sortState ??= ViewModels.Products.SortViewModel.Sort.NameAsc;
        }

        public void SetCookies(IResponseCookies cookies, string username, string selectedName, int? page, ViewModels.Products.SortViewModel.Sort? sortState, int? selectedMinPrice, int? selectedMaxPrice, int? selectedTypeId, int? selectedManufacturerId)
        {
            cookies.Append(username + "productSelectedName", selectedName);
            cookies.Append(username + "productPage", page.ToString());
            cookies.Append(username + "productSort", sortState.ToString());
            cookies.Append(username + "productSelectedMinPrice", selectedMinPrice.ToString());
            cookies.Append(username + "productSelectedMaxPrice", selectedMaxPrice.ToString());
            cookies.Append(username + "productSelectedType", selectedTypeId.ToString());
            cookies.Append(username + "productSelectedManufacturer", selectedManufacturerId.ToString());
        }
    }
}
