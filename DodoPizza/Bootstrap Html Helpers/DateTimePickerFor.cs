using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using WebGrease.Css.Extensions;

namespace System.Web.Mvc.Html
{
    public static partial class HtmlHelper
    {
        /// <summary>
        /// HTML Helper for rendering a DatePicker using 'Bootstrap.Datepicker' NuGet's package 1.3.0
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="expression">Lambda that will evaluate the object.</param>
        /// <param name="htmlAttributes">Custom html attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString DateTimePickerFor<TModel, TValue>
            (this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, [Optional] IDictionary<string, object> htmlAttributes)
        {
            // Get the Metadata from Model's DataAnnotations.
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

            // Main input.
            var input = new TagBuilder("input");

            // Setting attributes.
            input.Attributes.Add("id", metadata.PropertyName);
            input.Attributes.Add("name", metadata.PropertyName);
            input.Attributes.Add("type", "text");
            input.AddCssClass("form-control"); // Bootstrap's 3.1.1 input CSS class.

            input.MergeAttributes(htmlAttributes);

            var launchScript = new TagBuilder("script");

            launchScript.InnerHtml = "$(document).ready(function() { $('#" + metadata.PropertyName + "').datetimepicker(); })";

            // Adds the validation properties from the HelperMethods.HTMLHelper package.
            //Helpers.AddValidationProperties(self, expression, input);

            // If the current Model's value is not null, set the input's value to correspond the call. If it is, set to the current date.
            if (metadata.Model != null)
            {
                input.Attributes.Add("value", ((DateTime)metadata.Model).ToString("g"));
            }

            return new MvcHtmlString(input.ToString() + launchScript.ToString());
        }
    }
}