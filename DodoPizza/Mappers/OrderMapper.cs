using System.Collections.Generic;
using AutoMapper;
using DodoPizza.Models;
using DodoPizza.ViewModels;

namespace DodoPizza.Mappers
{
    public class OrderMapper : IMapper<Order,OrderView>
    {
        static OrderMapper()
        {
            Mapper.CreateMap<Order, OrderView>();
            Mapper.CreateMap<OrderView, Order>();
            Mapper.CreateMap<Product, ProductView>();
            Mapper.CreateMap<ProductView, Product>();
        }

        public OrderView Map(Order value)
        {
            return Mapper.Map<OrderView>(value);
        }

        public Order Map(OrderView value)
        {
            return Mapper.Map<Order>(value);
        }

        public ICollection<OrderView> Map(ICollection<Order> value)
        {
            return Mapper.Map<ICollection<OrderView>>(value);
        }

        public ICollection<Order> Map(ICollection<OrderView> value)
        {
            return Mapper.Map<ICollection<Order>>(value);
        }
    }
}