using System;
using DodoPizza.Models;

namespace DodoPizza.ViewModels
{
    public class ProductView
    {
        public int ID { get; set; }

        public int OrderID { get; set; }

        public ProductStatus Status { get; set; }

        public String Description { get; set; }

        public DateTime UpdateTime { get; set; }

        public OrderView Order { get; set; }
    }
}