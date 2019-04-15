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
        }

        public List<BestCustomerVm> BestCustomerVms;
        public IEnumerable<ItemStock> ItemStocks;
        public IEnumerable<Purchase> Purchases;
        public IEnumerable<ItemsSoldVm> ItemsSoldVms;
    }
}
