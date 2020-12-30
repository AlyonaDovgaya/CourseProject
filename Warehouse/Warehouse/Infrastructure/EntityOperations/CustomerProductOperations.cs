using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Infrastructure.EntityOperations
{
    public class CustomerProductOperations
    {
        public IQueryable<Models.CustomerProduct> Filter(IQueryable<Models.CustomerProduct> customerProducts, string selectedCustomerName)
        {
            if (!string.IsNullOrEmpty(selectedCustomerName))
            {
                customerProducts = customerProducts.Where(p => p.Customer.Name.Contains(selectedCustomerName));
            }
            return customerProducts;
        }

        public IQueryable<Models.CustomerProduct> Sort(IQueryable<Models.CustomerProduct> customerProducts, ViewModels.CustomerProducts.SortViewModel.Sort sortState)
        {
            switch (sortState)
            {
                case ViewModels.CustomerProducts.SortViewModel.Sort.CustomerNameAsc:
                    customerProducts = customerProducts.OrderBy(p => p.Customer.Name);
                    break;
                case ViewModels.CustomerProducts.SortViewModel.Sort.CustomerNameDesc:
                    customerProducts = customerProducts.OrderByDescending(p => p.Customer.Name);
                    break;
            }
            return customerProducts;
        }

        public IQueryable<Models.CustomerProduct> Paging(IQueryable<Models.CustomerProduct> customerProducts, bool isFromFilter, int page, int pageSize)
        {
            if (isFromFilter)
            {
                page = 1;
            }
            return customerProducts.Skip(((int)page - 1) * pageSize).Take(pageSize);
        }

        public void GetFilterCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilterForm, ref string selectedCustomerName)
        {
            if (string.IsNullOrEmpty(selectedCustomerName))
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "customerProductSelectedCustomerName", out selectedCustomerName);
                }
            }
        }

        public void GetSortPagingCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilter, ref int? page, ref ViewModels.CustomerProducts.SortViewModel.Sort? sortState)
        {
            if (!isFromFilter)
            {
                if (page == null)
                {
                    if (cookies.TryGetValue(username + "customerProductPage", out string pageStr))
                    {
                        page = int.Parse(pageStr);
                    }
                }
                if (sortState == null)
                {
                    if (cookies.TryGetValue(username + "customerProductSort", out string sortStateStr))
                    {
                        sortState = (ViewModels.CustomerProducts.SortViewModel.Sort)Enum.Parse(typeof(ViewModels.CustomerProducts.SortViewModel.Sort), sortStateStr);
                    }
                }
            }
        }

        public void SetDefaultValuesIfNull(ref string selectedCustomerName, ref int? page, ref ViewModels.CustomerProducts.SortViewModel.Sort? sortState)
        {
            selectedCustomerName ??= "";
            page ??= 1;
            sortState ??= ViewModels.CustomerProducts.SortViewModel.Sort.CustomerNameAsc;
        }

        public void SetCookies(IResponseCookies cookies, string username, string selectedCustomerName, int? page, ViewModels.CustomerProducts.SortViewModel.Sort? sortState)
        {
            cookies.Append(username + "customerProductSelectedCustomerName", selectedCustomerName);
            cookies.Append(username + "customerProductPage", page.ToString());
            cookies.Append(username + "customerProductSort", sortState.ToString());
        }
    }
}
