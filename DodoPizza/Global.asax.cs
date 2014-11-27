using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using DodoPizza.App_Start;
using DodoPizza.DAL;
using DodoPizza.Mappers;
using DodoPizza.Services;

namespace DodoPizza
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<OrderMapper>().SingleInstance();
            builder.RegisterType<ProductMapper>().SingleInstance();

            builder.RegisterType<OrdersContext>().SingleInstance();

            builder.RegisterType<OrderService>().SingleInstance();
            builder.RegisterType<ProductService>().SingleInstance();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DatePickerHelperBundleConfig.RegisterBundles();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //filling database with fake data, remove before production!
            Database.SetInitializer(new OrdersInitializer());
        }
    }
}
