using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Infrastructure.EntityOperations
{
    public class ManufacturerOperations
    {
        public IQueryable<Models.Manufacturer> Filter(IQueryable<Models.Manufacturer> manufacturers, string selectedName)
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                manufacturers = manufacturers.Where(p => p.Name.Contains(selectedName));
            }
            return manufacturers;
        }

        public IQueryable<Models.Manufacturer> Sort(IQueryable<Models.Manufacturer> manufacturers, ViewModels.Manufacturers.SortViewModel.Sort sortState)
        {
            switch (sortState)
            {
                case ViewModels.Manufacturers.SortViewModel.Sort.NameAsc:
                    manufacturers = manufacturers.OrderBy(p => p.Name);
                    break;
                case ViewModels.Manufacturers.SortViewModel.Sort.NameDesc:
                    manufacturers = manufacturers.OrderByDescending(p => p.Name);
                    break;
            }
            return manufacturers;
        }

        public IQueryable<Models.Manufacturer> Paging(IQueryable<Models.Manufacturer> manufacturers, bool isFromFilter, int page, int pageSize)
        {
            if (isFromFilter)
            {
                page = 1;
            }
            return manufacturers.Skip(((int)page - 1) * pageSize).Take(pageSize);
        }

        public void GetFilterCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilterForm, ref string selectedName)
        {
            if (string.IsNullOrEmpty(selectedName))
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "manufacturerSelectedName", out selectedName);
                }
            }
        }

        public void GetSortPagingCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilter, ref int? page, ref ViewModels.Manufacturers.SortViewModel.Sort? sortState)
        {
            if (!isFromFilter)
            {
                if (page == null)
                {
                    if (cookies.TryGetValue(username + "manufacturerPage", out string pageStr))
                    {
                        page = int.Parse(pageStr);
                    }
                }
                if (sortState == null)
                {
                    if (cookies.TryGetValue(username + "manufacturerSort", out string sortStateStr))
                    {
                        sortState = (ViewModels.Manufacturers.SortViewModel.Sort)Enum.Parse(typeof(ViewModels.Manufacturers.SortViewModel.Sort), sortStateStr);
                    }
                }
            }
        }

        public void SetDefaultValuesIfNull(ref string selectedName, ref int? page, ref ViewModels.Manufacturers.SortViewModel.Sort? sortState)
        {
            selectedName ??= "";
            page ??= 1;
            sortState ??= ViewModels.Manufacturers.SortViewModel.Sort.NameAsc;
        }

        public void SetCookies(IResponseCookies cookies, string username, string selectedName, int? page, ViewModels.Manufacturers.SortViewModel.Sort? sortState)
        {
            cookies.Append(username + "manufacturerSelectedName", selectedName);
            cookies.Append(username + "manufacturerPage", page.ToString());
            cookies.Append(username + "manufacturerSort", sortState.ToString());
        }
    }
}
