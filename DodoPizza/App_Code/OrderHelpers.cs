using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DodoPizza.Models;
using DodoPizza.ViewModels;
using HtmlHelper = System.Web.Mvc.HtmlHelper;

namespace DodoPizza
{
    public static class OrderHelpers
    {
        public static MvcHtmlString PassToRestaurantLink(this HtmlHelper htmlHelper, OrderView model)
        {
            if (model.CanPassToRestaurant)
            {
                return htmlHelper.ActionLink("Passed to restaurant", "Restaurant", "Order", new {id = model.ID}, null);
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString DeliveredLink(this HtmlHelper htmlHelper, OrderView model)
        {
            if (model.CanFinishDelivery)
            {
                return htmlHelper.ActionLink("Delivered", "Delivery", "Order", new { id = model.ID }, null);
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString CourierForm(this HtmlHelper htmlHelper, OrderView model)
        {
            if (model.IsReadyForDelivery)
            {
                return htmlHelper.Partial("Courier", new CourierView(), new ViewDataDictionary { { "id", model.ID } });
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString CreateProductLink(this HtmlHelper htmlHelper, OrderView model)
        {
            if (model.CanCreateProduct)
            {
                return htmlHelper.ActionLink("Create New", "Create", "Product", new { id = model.ID }, null);
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString ProductActionLink(this HtmlHelper htmlHelper, ProductView model)
        {
            if (model.Status == ProductStatus.New)
            {
                return htmlHelper.ActionLink("Start progress", "Start", "Product", new { id = model.ID }, null);
            }
            if (model.Status == ProductStatus.InProgress)
            {
                return htmlHelper.ActionLink("Finish progress", "Finish", "Product", new { id = model.ID }, null);
            }
            return MvcHtmlString.Empty;
        }
    }
}