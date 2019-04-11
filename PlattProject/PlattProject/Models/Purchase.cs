using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PlattProject.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int NumberPurchased { get; set; }
        public DateTime PurchaseDate { get; set; }

        public DateTime? ShipDate { get; set; } //null if not yet shipped

        [ForeignKey("Warehouse")]
        public int? WarehouseId { get; set; } //null until warehouse has been selected
        


    }

    
}
