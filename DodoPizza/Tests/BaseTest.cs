using System.Data.Entity;
using Autofac;
using DodoPizza.DAL;
using DodoPizza.Mappers;
using DodoPizza.Models;
using DodoPizza.Services;
using Moq;

namespace DodoPizza.Tests
{
    public class BaseTest
    {
        protected IContainer Container = null;
        protected readonly Mock<OrdersContext> Context = new Mock<OrdersContext>();

        public BaseTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<OrderMapper>().SingleInstance();
            builder.RegisterType<ProductMapper>().SingleInstance();
            builder.RegisterType<OrderService>().SingleInstance();
            builder.RegisterType<ProductService>().SingleInstance();

            Context.Setup(c => c.Orders).Returns(Mock.Of<DbSet<Order>>());
            Context.Setup(c => c.Products).Returns(Mock.Of<DbSet<Product>>());
            builder.RegisterInstance(Context.Object).As<OrdersContext>().SingleInstance();

            Container = builder.Build();
        }
    }
}