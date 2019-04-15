using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattProject.ViewModels
{
    public class BestCustomerVm
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int NumberOfOrders { get; set; }
        public int AmountSpent { get; set; }
    }
}
