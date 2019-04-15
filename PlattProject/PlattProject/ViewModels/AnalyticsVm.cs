using PlattProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattProject.ViewModels
{
    public class AnalyticsVm
    {
        public AnalyticsVm()
        {
            BestCustomerVms = new List<BestCustomerVm>();
            ItemRestocks = new List<ItemRestockVm>();
            LargestOrders = new List<LargestOrderVm>();
            ItemsSoldVms = new List<ItemsSoldVm>();
        }

        public List<BestCustomerVm> BestCustomerVms;
        public List<ItemRestockVm> ItemRestocks;
        public List<LargestOrderVm> LargestOrders;
        public List<ItemsSoldVm> ItemsSoldVms;
    }
}
