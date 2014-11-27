using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using DodoPizza.Models;
using DodoPizza.Services;
using DodoPizza.ViewModels;

namespace DodoPizza.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: Order
        public ActionResult Index(OrderStatus? status)
        {
            if (!status.HasValue)
            {
                return View(_orderService.FindAllOrders());
            }
            return View("Index", _orderService.FindOrdersForStatus(status.Value));
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = _orderService.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/Restaurant/5
        public ActionResult Restaurant(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = _orderService.PassToRestaurant(id);
            return RedirectToAction("Details", new { id = order.ID});
        }

        // GET: Order/Restaurant/5
        public ActionResult Delivery(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = _orderService.MarkDelivered(id);
            return RedirectToAction("Details", new { id = order.ID });
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DeliveryDate,DeliveryTime,Type")] OrderView order)
        {
            if (ModelState.IsValid)
            {
                _orderService.CreateOrder(order);
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // POST: Order/Assign/2
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign([Bind(Include = "Name")] CourierView courier, int? id)
        {
            if (ModelState.IsValid)
            {
                _orderService.AssignCourier(id, courier);
                return RedirectToAction("Details", new { id });
            }

            return View(_orderService.Find(id));
        }
    }
}
