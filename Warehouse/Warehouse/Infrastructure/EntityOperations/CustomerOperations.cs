using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Infrastructure.EntityOperations
{
    public class CustomerOperations
    {
        public IQueryable<Models.Customer> Filter(IQueryable<Models.Customer> customers, string selectedName, int? selectedMinOrderQuantity, int? selectedMaxOrderQuantity, IQueryable<Models.CustomerProduct> customerProducts)
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                customers = customers.Where(p => p.Name.Contains(selectedName));
            }

            var customerOrderQuantities = customerProducts.Join(customers, cp => cp.CustomerId, c => c.Id, (cp, c) => new
            {
                Customer = c.Id,
                cp.Quantity
            });

            if (selectedMinOrderQuantity != null)
            {
                customers = customers
                    .Where(c => customerOrderQuantities
                        .Where(oq => oq.Customer == c.Id)
                        .Select(oq => oq.Quantity)
                        .Sum() >= selectedMinOrderQuantity);
            }

            if (selectedMaxOrderQuantity != null)
            {
                customers = customers
                    .Where(c => customerOrderQuantities
                        .Where(oq => oq.Customer == c.Id)
                        .Select(oq => oq.Quantity)
                        .Sum() <= selectedMaxOrderQuantity);
            }

            return customers;
        }

        public IQueryable<Models.Customer> Sort(IQueryable<Models.Customer> customers, ViewModels.Customers.SortViewModel.Sort sortState)
        {
            switch (sortState)
            {
                case ViewModels.Customers.SortViewModel.Sort.NameAsc:
                    customers = customers.OrderBy(p => p.Name);
                    break;
                case ViewModels.Customers.SortViewModel.Sort.NameDesc:
                    customers = customers.OrderByDescending(p => p.Name);
                    break;
            }
            return customers;
        }

        public IQueryable<Models.Customer> Paging(IQueryable<Models.Customer> customers, bool isFromFilter, int page, int pageSize)
        {
            if (isFromFilter)
            {
                page = 1;
            }
            return customers.Skip(((int)page - 1) * pageSize).Take(pageSize);
        }

        public void GetFilterCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilterForm, ref string selectedName, ref int? selectedMinOrderQuantity, ref int? selectedMaxOrderQuantity)
        {
            if (string.IsNullOrEmpty(selectedName))
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "customerSelectedName", out selectedName);
                }
            }

            if (selectedMinOrderQuantity == null)
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "customerSelectedMinQuantity", out string selectedMinQuantityStr);
                    selectedMinOrderQuantity = string.IsNullOrEmpty(selectedMinQuantityStr)
                        ? null
                        : (int?)int.Parse(selectedMinQuantityStr);
                }
            }

            if (selectedMaxOrderQuantity == null)
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "customerSelectedMaxQuantity", out string selectedMaxQuantityStr);
                    selectedMaxOrderQuantity = string.IsNullOrEmpty(selectedMaxQuantityStr)
                        ? null
                        : (int?)int.Parse(selectedMaxQuantityStr);
                }
            }
        }

        public void GetSortPagingCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilter, ref int? page, ref ViewModels.Customers.SortViewModel.Sort? sortState)
        {
            if (!isFromFilter)
            {
                if (page == null)
                {
                    if (cookies.TryGetValue(username + "customerPage", out string pageStr))
                    {
                        page = int.Parse(pageStr);
                    }
                }
                if (sortState == null)
                {
                    if (cookies.TryGetValue(username + "customerSort", out string sortStateStr))
                    {
                        sortState = (ViewModels.Customers.SortViewModel.Sort)Enum.Parse(typeof(ViewModels.Customers.SortViewModel.Sort), sortStateStr);
                    }
                }
            }
        }

        public void SetDefaultValuesIfNull(ref string selectedName, ref int? page, ref ViewModels.Customers.SortViewModel.Sort? sortState)
        {
            selectedName ??= "";
            page ??= 1;
            sortState ??= ViewModels.Customers.SortViewModel.Sort.NameAsc;
        }

        public void SetCookies(IResponseCookies cookies, string username, string selectedName, int? page, ViewModels.Customers.SortViewModel.Sort? sortState, int? selectedMinOrderQuantity, int? selectedMaxOrderQuantity)
        {
            cookies.Append(username + "customerSelectedName", selectedName);
            cookies.Append(username + "customerPage", page.ToString());
            cookies.Append(username + "customerSort", sortState.ToString());
            cookies.Append(username + "customerSelectedMinQuantity", selectedMinOrderQuantity.ToString());
            cookies.Append(username + "customerSelectedMaxQuantity", selectedMaxOrderQuantity.ToString());
        }
    }
}
