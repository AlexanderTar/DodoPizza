using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DodoPizza.Models
{
    public enum OrderStatus
    {
        Queued,
        New,
        InProgress,
        Ready,
        InDelivery,
        Delivered,
        InRestaurant
    }

    public enum OrderType
    {
        Delivery,
        Restaurant
    }

    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public OrderType Type { get; set; }

        public DateTime UpdateTime { get; set; }

        public String Courier { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}