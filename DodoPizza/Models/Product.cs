using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DodoPizza.Models
{
    public enum ProductStatus
    {
        Queued,
        New,
        InProgress,
        Ready
    }

    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int OrderID { get; set; }
        public ProductStatus Status { get; set; }
        public String Description { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual Order Order { get; set; }
    }
}