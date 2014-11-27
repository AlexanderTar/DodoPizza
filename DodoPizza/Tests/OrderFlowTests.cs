using System;
using System.Collections.Generic;
using Autofac;
using DodoPizza.Models;
using DodoPizza.Services;
using DodoPizza.ViewModels;
using Moq;
using NUnit.Framework;

namespace DodoPizza.Tests
{
    //basic tests for order flow actions
    //much things to do
    //but helps to test basic scenario
    [TestFixture]
    public class OrderFlowTests : BaseTest
    {
        [Test]
        public void CreateOrderTest()
        {
            var orderService = Container.Resolve<OrderService>();
            var orderView = new OrderView();
            var result = orderService.CreateOrder(orderView);
            Assert.AreEqual(OrderStatus.New, result.Status);
            orderView.DeliveryDate = DateTime.Now;
            result = orderService.CreateOrder(orderView);
            Assert.AreEqual(OrderStatus.Queued, result.Status);
        }

        [Test]
        public void PassToRestaurantTest()
        {
            var orderService = Container.Resolve<OrderService>();
            //Moq can't mock public properties for objects such as "CanPassToRestaurant"
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
                .Returns(() => new Order
            {
                Status = OrderStatus.Ready,
                Type = OrderType.Restaurant
            });
            var result = orderService.PassToRestaurant(0);
            Assert.AreEqual(OrderStatus.InRestaurant, result.Status);
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
                .Returns(() => new Order());
            result = orderService.PassToRestaurant(0);
            Assert.AreNotEqual(OrderStatus.InRestaurant, result.Status);
        }

        [Test]
        public void MarkDeliveredTest()
        {
            var orderService = Container.Resolve<OrderService>();
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
                .Returns(() => new Order
            {
                Status = OrderStatus.InDelivery,
                Type = OrderType.Delivery
            });
            var result = orderService.MarkDelivered(0);
            Assert.AreEqual(OrderStatus.Delivered, result.Status);
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
                .Returns(() => new Order()); 
            result = orderService.MarkDelivered(0);
            Assert.AreNotEqual(OrderStatus.Delivered, result.Status);
        }

        [Test]
        public void AssignCourierTest()
        {
            var orderService = Container.Resolve<OrderService>();
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
                .Returns(() => new Order
            {
                Status = OrderStatus.Ready,
                Type = OrderType.Delivery
            });
            var result = orderService.AssignCourier(0, new CourierView {Name = "test"});
            Assert.AreEqual(OrderStatus.InDelivery, result.Status); 
            result = orderService.AssignCourier(1, new CourierView());
            Assert.AreNotEqual(OrderStatus.InDelivery, result.Status);
        }

        [Test]
        public void ProductInProgressTest()
        {
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
                .Returns(() =>
                {
                    var order = new Order
                    {
                        Status = OrderStatus.New,
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Status = ProductStatus.InProgress
                            }
                        }
                    };
                    return order;
                });
            var orderService = Container.Resolve<OrderService>();
            var result = orderService.UpdateOrderIfNeeded(0);
            Assert.AreEqual(OrderStatus.InProgress, result.Status);
        }

        [Test]
        public void ProductReadyTest()
        {
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
                .Returns(() =>
                {
                    var order = new Order
                    {
                        Status = OrderStatus.InProgress,
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Status = ProductStatus.Ready
                            }
                        }
                    };
                    return order;
                });
            var orderService = Container.Resolve<OrderService>();
            var result = orderService.UpdateOrderIfNeeded(0);
            Assert.AreEqual(OrderStatus.Ready, result.Status);
        }
    }
}