using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Infrastructure.EntityOperations
{
    public class EmployeeOperations
    {
        public IQueryable<Models.Employee> Filter(IQueryable<Models.Employee> employees, string selectedName)
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                employees = employees.Where(p => p.Name.Contains(selectedName));
            }
            return employees;
        }

        public IQueryable<Models.Employee> Sort(IQueryable<Models.Employee> employees, ViewModels.Employees.SortViewModel.Sort sortState)
        {
            switch (sortState)
            {
                case ViewModels.Employees.SortViewModel.Sort.NameAsc:
                    employees = employees.OrderBy(p => p.Name);
                    break;
                case ViewModels.Employees.SortViewModel.Sort.NameDesc:
                    employees = employees.OrderByDescending(p => p.Name);
                    break;
            }
            return employees;
        }

        public IQueryable<Models.Employee> Paging(IQueryable<Models.Employee> employees, bool isFromFilter, int page, int pageSize)
        {
            if (isFromFilter)
            {
                page = 1;
            }
            return employees.Skip(((int)page - 1) * pageSize).Take(pageSize);
        }

        public void GetFilterCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilterForm, ref string selectedName)
        {
            if (string.IsNullOrEmpty(selectedName))
            {
                if (!isFromFilterForm)
                {
                    cookies.TryGetValue(username + "employeeSelectedName", out selectedName);
                }
            }
        }

        public void GetSortPagingCookiesForUserIfNull(IRequestCookieCollection cookies, string username, bool isFromFilter, ref int? page, ref ViewModels.Employees.SortViewModel.Sort? sortState)
        {
            if (!isFromFilter)
            {
                if (page == null)
                {
                    if (cookies.TryGetValue(username + "employeePage", out string pageStr))
                    {
                        page = int.Parse(pageStr);
                    }
                }
                if (sortState == null)
                {
                    if (cookies.TryGetValue(username + "employeeSort", out string sortStateStr))
                    {
                        sortState = (ViewModels.Employees.SortViewModel.Sort)Enum.Parse(typeof(ViewModels.Employees.SortViewModel.Sort), sortStateStr);
                    }
                }
            }
        }

        public void SetDefaultValuesIfNull(ref string selectedName, ref int? page, ref ViewModels.Employees.SortViewModel.Sort? sortState)
        {
            selectedName ??= "";
            page ??= 1;
            sortState ??= ViewModels.Employees.SortViewModel.Sort.NameAsc;
        }

        public void SetCookies(IResponseCookies cookies, string username, string selectedName, int? page, ViewModels.Employees.SortViewModel.Sort? sortState)
        {
            cookies.Append(username + "employeeSelectedName", selectedName);
            cookies.Append(username + "employeePage", page.ToString());
            cookies.Append(username + "employeeSort", sortState.ToString());
        }
    }
}
