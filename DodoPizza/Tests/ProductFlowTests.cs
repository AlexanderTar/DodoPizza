using Autofac;
using DodoPizza.Models;
using DodoPizza.Services;
using DodoPizza.ViewModels;
using Moq;
using NUnit.Framework;

namespace DodoPizza.Tests
{
    //basic tests for product flow actions
    //much things to do
    //but helps to test basic scenario
    [TestFixture]
    public class ProductFlowTests : BaseTest
    {
        [Test]
        public void CreateProductTest()
        {
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
               .Returns(() => new Order
               {
                   Status = OrderStatus.New
               });
            var productService = Container.Resolve<ProductService>();
            var productView = new ProductView();
            var result = productService.CreateProduct(productView);
            Assert.AreEqual(ProductStatus.New, result.Status);
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
               .Returns(() => new Order
               {
                   Status = OrderStatus.Queued
               });
            result = productService.CreateProduct(productView);
            Assert.AreEqual(ProductStatus.Queued, result.Status);
        }

        [Test]
        public void StartProgressTest()
        {
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
               .Returns(() => new Order());
            Context.Setup(c => c.Products.Find(It.IsAny<object[]>()))
               .Returns(() => new Product
               {
                   Status = ProductStatus.New
               });
            var productService = Container.Resolve<ProductService>();
            var result = productService.StartProgress(0);
            Assert.AreEqual(ProductStatus.InProgress, result.Status);
            Context.Setup(c => c.Products.Find(It.IsAny<object[]>()))
               .Returns(() => new Product
               {
                   Status = ProductStatus.Queued
               });
            result = productService.StartProgress(0);
            Assert.AreNotEqual(ProductStatus.InProgress, result.Status);
        }

        [Test]
        public void FinishProgressTest()
        {
            Context.Setup(c => c.Orders.Find(It.IsAny<object[]>()))
               .Returns(() => new Order());
            Context.Setup(c => c.Products.Find(It.IsAny<object[]>()))
               .Returns(() => new Product
               {
                   Status = ProductStatus.InProgress
               });
            var productService = Container.Resolve<ProductService>();
            var result = productService.FinishProgress(0);
            Assert.AreEqual(ProductStatus.Ready, result.Status);
            Context.Setup(c => c.Products.Find(It.IsAny<object[]>()))
               .Returns(() => new Product
               {
                   Status = ProductStatus.New
               });
            result = productService.StartProgress(0);
            Assert.AreNotEqual(ProductStatus.Ready, result.Status);
        }
    }
}