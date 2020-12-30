using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Warehouse.Models;

namespace Warehouse.Infrastructure.ViewModels.Products
{
    public class FilterViewModel
    {
        public string SelectedName { get; set; }
        public int? SelectedMinPrice { get; set; }
        public int? SelectedMaxPrice { get; set; }
        public int? SelectedManufacturerId { get; set; }
        public int? SelectedTypeId { get; set; }

        public SelectList Types { get; set; }
        public SelectList Manufacturers { get; set; }


        public FilterViewModel(string selectedName, int? selectedMinPrice, int? selectedMaxPrice, int? selectedTypeId, int? selectedManufacturerId, List<Models.ProductType> types, List<Models.Manufacturer> manufacturers)
        {
            SelectedName = selectedName;
            SelectedMinPrice = selectedMinPrice;
            SelectedMaxPrice = selectedMaxPrice;
            SelectedManufacturerId = selectedManufacturerId;
            SelectedTypeId = selectedTypeId;

            types.Insert(0, new ProductType {Id = 0, Name = "All"});
            manufacturers.Insert(0, new Manufacturer {Id = 0, Name = "All"});

            Types = new SelectList(types, "Id", "Name", selectedTypeId);
            Manufacturers = new SelectList(manufacturers, "Id", "Name", selectedManufacturerId);
        }
    }
}
