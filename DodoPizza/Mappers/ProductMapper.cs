using System.Collections.Generic;
using AutoMapper;
using DodoPizza.Models;
using DodoPizza.ViewModels;

namespace DodoPizza.Mappers
{
    public class ProductMapper : IMapper<Product,ProductView>
    {
        static ProductMapper()
        {
            Mapper.CreateMap<Product, ProductView>();
            Mapper.CreateMap<ProductView, Product>();
            Mapper.CreateMap<Product, ProductView>();
            Mapper.CreateMap<ProductView, Product>();
        }

        public ProductView Map(Product value)
        {
            return Mapper.Map<ProductView>(value);
        }

        public Product Map(ProductView value)
        {
            return Mapper.Map<Product>(value);
        }

        public ICollection<ProductView> Map(ICollection<Product> value)
        {
            return Mapper.Map<ICollection<ProductView>>(value);
        }

        public ICollection<Product> Map(ICollection<ProductView> value)
        {
            return Mapper.Map<ICollection<Product>>(value);
        }
    }
}