using System;
using System.Collections.Generic;
using System.Linq;
using DodoPizza.DAL;
using DodoPizza.Mappers;
using DodoPizza.Models;
using DodoPizza.ViewModels;
using FluentScheduler;
using WebGrease.Css.Extensions;

namespace DodoPizza.Services
{
    public class OrderService : Registry
    {
        private readonly OrdersContext _db;
        private readonly OrderMapper _orderMapper;

        public OrderService(OrdersContext db,
            OrderMapper orderMapper)
        {
            _db = db;
            _orderMapper = orderMapper;
        }

        public ICollection<OrderView> FindOrdersForStatus(OrderStatus status)
        {
            UpdateQueuedOrders();
            return _orderMapper.Map(_db.Orders.Where(o => o.Status == status).ToList());
        }

        public ICollection<OrderView> FindAllOrders()
        {
            UpdateQueuedOrders();
            return _orderMapper.Map(_db.Orders.ToList());
        }

        public OrderView CreateOrder(OrderView orderView)
        {
            orderView.Status = orderView.DeliveryDate != null
                    ? OrderStatus.Queued
                    : OrderStatus.New;
            orderView.UpdateTime = DateTime.Now;
            _db.Orders.Add(_orderMapper.Map(orderView));
            _db.SaveChanges();
            return orderView;
        }

        public OrderView PassToRestaurant(int? id)
        {
            var order = _db.Orders.Find(id);
            if (CanPassToRestaurant(order))
            {
                order.Status = OrderStatus.InRestaurant;
                UpdateOrder(order);
            }
            return _orderMapper.Map(order);
        }

        public OrderView MarkDelivered(int? id)
        {
            var order = _db.Orders.Find(id);
            if (CanFinishDelivery(order))
            {
                order.Status = OrderStatus.Delivered;
                UpdateOrder(order);
            }
            return _orderMapper.Map(order);
        }

        public OrderView AssignCourier(int? id, CourierView courier)
        {
            var order = _db.Orders.Find(id);
            if (IsReadyForDelivery(order) &&
                !String.IsNullOrEmpty(courier.Name))
            {
                order.Courier = courier.Name;
                order.Status = OrderStatus.InDelivery;
                UpdateOrder(order);
            }
            return _orderMapper.Map(order);
        }

        private void UpdateQueuedOrders()
        {
            _db.Orders.ForEach(o => UpdateQueuedOrderIfNeeded(o));
        }

        private Order UpdateQueuedOrderIfNeeded(Order order)
        {
            if (DateTime.Now >= order.DeliveryDate &&
                order.Status == OrderStatus.Queued)
            {
                order = UpdateQueuedOrder(order);
            }
            return order;
        }

        private Order UpdateQueuedOrder(Order order)
        {
            order.Status = OrderStatus.New;
            order.UpdateTime = DateTime.Now;
            order.Products.ForEach(p =>
            {
                p.Status = ProductStatus.New;
                p.UpdateTime = DateTime.Now;
            });
            _db.SaveChangesAsync();
            return order;
        }

        public Order UpdateOrderIfNeeded(int? orderId)
        {
            var order = _db.Orders.Find(orderId);
            if (order.Status == OrderStatus.New &&
                order.Products.Any(p => p.Status == ProductStatus.InProgress))
            {
                order.Status = OrderStatus.InProgress;
                order = UpdateOrder(order);
            }
            if (order.Status == OrderStatus.InProgress &&
                order.Products.All(p => p.Status == ProductStatus.Ready))
            {
                order.Status = OrderStatus.Ready;
                order = UpdateOrder(order);
            }
            return order;
        }

        private Order UpdateOrder(Order order)
        {
            order.UpdateTime = DateTime.Now;
            _db.SaveChanges();
            return order;
        }

        public OrderView Find(int? id)
        {
            var order = _db.Orders.Find(id);
            return _orderMapper.Map(UpdateQueuedOrderIfNeeded(order));
        }

        private bool IsReadyForDelivery(Order order)
        {
            return _orderMapper.Map(order).IsReadyForDelivery;
        }

        private bool CanPassToRestaurant(Order order)
        {
            return _orderMapper.Map(order).CanPassToRestaurant;
        }

        private bool CanFinishDelivery(Order order)
        {
            return _orderMapper.Map(order).CanFinishDelivery;
        }
    }
}