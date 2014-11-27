using DodoPizza.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DodoPizza.DAL
{
    public class OrdersInitializer : DropCreateDatabaseAlways<OrdersContext>
    {
        protected override void Seed(OrdersContext context)
        {
            var orders = new List<Order>
            {
                new Order {Status = OrderStatus.New, Type = OrderType.Delivery, UpdateTime = DateTime.Now},
                new Order {Status = OrderStatus.New, Type = OrderType.Restaurant, UpdateTime = DateTime.Now},
                new Order {Status = OrderStatus.Queued, Type = OrderType.Delivery, UpdateTime = DateTime.Now, DeliveryDate = DateTime.Now.AddSeconds(30)}
            };
            orders.ForEach(o => context.Orders.Add(o));
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product {Status = ProductStatus.New, OrderID = 1, UpdateTime = DateTime.Now},
                new Product {Status = ProductStatus.New, OrderID = 1, UpdateTime = DateTime.Now},
                new Product {Status = ProductStatus.New, OrderID = 2, UpdateTime = DateTime.Now},
                new Product {Status = ProductStatus.New, OrderID = 2, UpdateTime = DateTime.Now},
                new Product {Status = ProductStatus.Queued, OrderID = 3, UpdateTime = DateTime.Now},
                new Product {Status = ProductStatus.Queued, OrderID = 3, UpdateTime = DateTime.Now}
            };
            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();
        }
    }
}