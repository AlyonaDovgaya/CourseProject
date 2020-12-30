using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Infrastructure.EntityOperations
{
    public class DeliveryMethodOperations
    {
        public IQueryable<Models.DeliveryMethod> Filter(IQueryable<Models.DeliveryMethod> deliveryMethods, string selectedName)
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                deliveryMethods = deliveryMethods.Where(p => p.Name.Contains(selectedName));
            }
            return deliveryMethods;
        }

        public IQueryable<Models.DeliveryMethod> Sort(IQueryable<Models.DeliveryMethod> deliveryMethods, ViewModels.DeliveryMethods.SortViewModel.Sort sortState)
        {
            switch (sortState)
            {
                case ViewModels.DeliveryMethods.SortViewModel.Sort.NameAsc:
                    deliveryMethods = deliveryMethods.OrderBy(p => p.Name);
                    break;
                case ViewModels.DeliveryMethods.SortViewModel.Sort.NameDesc:
                    deliveryMethods = deliveryMethods.OrderByDescending(p => p.Name);
                    break;
            }
            return deliveryMethods;
        }

        public IQueryable<Models.DeliveryMethod> Paging(IQueryable<Models.DeliveryMethod> deliveryMethods, bool isFromFilter, int page, int pageSize)
        {
            if (isFromFilter)
            {
                page = 1;
            }
            return deliveryMethods.Skip(((int)page - 1) * pageSize).Take(pageSize);
        }

        public void GetFilterCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilterForm, ref string selectedName)
        {
            if (string.IsNullOrEmpty(selectedName))
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "deliveryMethodSelectedName", out selectedName);
                }
            }
        }

        public void GetSortPagingCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilter, ref int? page, ref ViewModels.DeliveryMethods.SortViewModel.Sort? sortState)
        {
            if (!isFromFilter)
            {
                if (page == null)
                {
                    if (cookies.TryGetValue(username + "deliveryMethodPage", out string pageStr))
                    {
                        page = int.Parse(pageStr);
                    }
                }
                if (sortState == null)
                {
                    if (cookies.TryGetValue(username + "deliveryMethodSort", out string sortStateStr))
                    {
                        sortState = (ViewModels.DeliveryMethods.SortViewModel.Sort)Enum.Parse(typeof(ViewModels.DeliveryMethods.SortViewModel.Sort), sortStateStr);
                    }
                }
            }
        }

        public void SetDefaultValuesIfNull(ref string selectedName, ref int? page, ref ViewModels.DeliveryMethods.SortViewModel.Sort? sortState)
        {
            selectedName ??= "";
            page ??= 1;
            sortState ??= ViewModels.DeliveryMethods.SortViewModel.Sort.NameAsc;
        }

        public void SetCookies(IResponseCookies cookies, string username, string selectedName, int? page, ViewModels.DeliveryMethods.SortViewModel.Sort? sortState)
        {
            cookies.Append(username + "deliveryMethodSelectedName", selectedName);
            cookies.Append(username + "deliveryMethodPage", page.ToString());
            cookies.Append(username + "deliveryMethodSort", sortState.ToString());
        }
    }
}
