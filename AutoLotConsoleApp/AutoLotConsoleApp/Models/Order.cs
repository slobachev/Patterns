using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace AutoLotDAL.Models
{
    public partial class Order
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public int CarId { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("CarId")]
        public virtual Inventory Car { get; set; }
    }
}
