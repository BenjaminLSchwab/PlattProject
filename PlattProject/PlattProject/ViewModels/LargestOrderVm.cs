using PlattProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattProject.ViewModels
{
    public class LargestOrderVm
    {
        public Purchase Purchase { get; set; }
        public string ItemName { get; set; }
        public string UserName { get; set; }
    }
}
