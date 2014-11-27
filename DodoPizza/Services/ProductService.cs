using System;
using DodoPizza.DAL;
using DodoPizza.Mappers;
using DodoPizza.Models;
using DodoPizza.ViewModels;

namespace DodoPizza.Services
{
    public class ProductService
    {
        private readonly OrdersContext _db;
        private readonly ProductMapper _productMapper;
        private readonly OrderService _orderService;

        public ProductService(OrdersContext db,
            ProductMapper productMapper,
            OrderService orderService)
        {
            _db = db;
            _productMapper = productMapper;
            _orderService = orderService;
        }

        public ProductView CreateProduct(ProductView productView)
        {
            var order = _orderService.Find(productView.OrderID);
            productView.Status = order.Status == OrderStatus.Queued ? ProductStatus.Queued : ProductStatus.New;
            productView.UpdateTime = DateTime.Now;
            var product = _productMapper.Map(productView);
            _db.Products.Add(product);
            _db.SaveChanges();
            return _productMapper.Map(product);
        }

        public ProductView StartProgress(int? id)
        {
            var product = _db.Products.Find(id);
            if (product.Status == ProductStatus.New)
            {
                product.Status = ProductStatus.InProgress;
                product = UpdateProduct(product);
            }
            return _productMapper.Map(product);
        }

        public ProductView FinishProgress(int? id)
        {
            var product = _db.Products.Find(id);
            if (product.Status == ProductStatus.InProgress)
            {
                product.Status = ProductStatus.Ready;
                product = UpdateProduct(product);
            }
            return _productMapper.Map(product);
        }

        private Product UpdateProduct(Product product)
        {
            product.UpdateTime = DateTime.Now;
            _db.SaveChanges();
            _orderService.UpdateOrderIfNeeded(product.OrderID);
            return product;
        }

        public ProductView Find(int? id)
        {
            return _productMapper.Map(_db.Products.Find(id));
        }
    }
}