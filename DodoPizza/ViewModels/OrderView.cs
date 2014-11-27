using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DodoPizza.Models;

namespace DodoPizza.ViewModels
{
    public class OrderView
    {
        public int ID { get; set; }

        public OrderStatus Status { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Delivery date")]
        public DateTime? DeliveryDate { get; set; }

        public OrderType Type { get; set; }

        public DateTime UpdateTime { get; set; }

        public String Courier { get; set; }

        public ICollection<ProductView> Products { get; set; }

        public bool CanCreateProduct
        {
            get
            {
                return Status == OrderStatus.New ||
                       Status == OrderStatus.Queued;
            }
        }

        public bool IsReadyForDelivery
        {
            get
            {
                return Status == OrderStatus.Ready &&
                       Type == OrderType.Delivery;
            }
        }

        public bool CanPassToRestaurant
        {
            get
            {
                return Status == OrderStatus.Ready &&
                       Type == OrderType.Restaurant;
            }
        }

        public bool CanFinishDelivery
        {
            get
            {
                return Status == OrderStatus.InDelivery &&
                       Type == OrderType.Delivery;
            }
        }
    }
}