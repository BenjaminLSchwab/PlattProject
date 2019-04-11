using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlattProject.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }
        public int MyProperty { get; set; }

        public ICollection<ItemStock> ItemStocks { get; set; }
    }
}
