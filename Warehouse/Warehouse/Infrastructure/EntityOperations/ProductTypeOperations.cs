using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Infrastructure.EntityOperations
{
    public class ProductTypeOperations
    {
        public IQueryable<Models.ProductType> Filter(IQueryable<Models.ProductType> productTypes, string selectedName)
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                productTypes = productTypes.Where(p => p.Name.Contains(selectedName));
            }
            return productTypes;
        }

        public IQueryable<Models.ProductType> Sort(IQueryable<Models.ProductType> productTypes, ViewModels.ProductTypes.SortViewModel.Sort sortState)
        {
            switch (sortState)
            {
                case ViewModels.ProductTypes.SortViewModel.Sort.NameAsc:
                    productTypes = productTypes.OrderBy(p => p.Name);
                    break;
                case ViewModels.ProductTypes.SortViewModel.Sort.NameDesc:
                    productTypes = productTypes.OrderByDescending(p => p.Name);
                    break;
            }
            return productTypes;
        }

        public IQueryable<Models.ProductType> Paging(IQueryable<Models.ProductType> productTypes, bool isFromFilter, int page, int pageSize)
        {
            if (isFromFilter)
            {
                page = 1;
            }
            return productTypes.Skip(((int)page - 1) * pageSize).Take(pageSize);
        }

        public void GetFilterCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilterForm, ref string selectedName)
        {
            if (string.IsNullOrEmpty(selectedName))
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "productTypeSelectedName", out selectedName);
                }
            }
        }

        public void GetSortPagingCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilter, ref int? page, ref ViewModels.ProductTypes.SortViewModel.Sort? sortState)
        {
            if (!isFromFilter)
            {
                if (page == null)
                {
                    if (cookies.TryGetValue(username + "productTypePage", out string pageStr))
                    {
                        page = int.Parse(pageStr);
                    }
                }
                if (sortState == null)
                {
                    if (cookies.TryGetValue(username + "productTypeSort", out string sortStateStr))
                    {
                        sortState = (ViewModels.ProductTypes.SortViewModel.Sort)Enum.Parse(typeof(ViewModels.ProductTypes.SortViewModel.Sort), sortStateStr);
                    }
                }
            }
        }

        public void SetDefaultValuesIfNull(ref string selectedName, ref int? page, ref ViewModels.ProductTypes.SortViewModel.Sort? sortState)
        {
            selectedName ??= "";
            page ??= 1;
            sortState ??= ViewModels.ProductTypes.SortViewModel.Sort.NameAsc;
        }

        public void SetCookies(IResponseCookies cookies, string username, string selectedName, int? page, ViewModels.ProductTypes.SortViewModel.Sort? sortState)
        {
            cookies.Append(username + "productTypeSelectedName", selectedName);
            cookies.Append(username + "productTypePage", page.ToString());
            cookies.Append(username + "productTypeSort", sortState.ToString());
        }
    }
}
