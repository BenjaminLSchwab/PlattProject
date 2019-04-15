using PlattProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattProject.ViewModels
{
    public class ItemRestockVm
    {
        public ItemStock ItemStock { get; set; }
        public string ItemName { get; set; }
        public string WarehouseAddress { get; set; }
    }
}
