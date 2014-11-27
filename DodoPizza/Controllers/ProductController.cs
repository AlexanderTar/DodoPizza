using System.Net;
using System.Web.Mvc;
using DodoPizza.Services;
using DodoPizza.ViewModels;

namespace DodoPizza.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: Product/Create/2
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new ProductView {OrderID = id.Value};
            return View(model);
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,OrderID,Status")] ProductView product)
        {
            if (ModelState.IsValid)
            {
                _productService.CreateProduct(product);
                return RedirectToAction("Details", "Order", new { id = product.OrderID});
            }

            return View(product);
        }

        // GET: Product/Start/2
        public ActionResult Start(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = _productService.StartProgress(id);
            return RedirectToAction("Details", "Order", new { id = product.OrderID });
        }

        // GET: Product/Finish/2
        public ActionResult Finish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = _productService.FinishProgress(id);
            return RedirectToAction("Details", "Order", new { id = product.OrderID });
        }
    }
}
