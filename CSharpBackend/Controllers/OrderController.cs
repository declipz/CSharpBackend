using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CSharpBackend.Database;
using CSharpBackend.Models;

namespace CSharpBackend.Controllers
{
    public class OrderController : ApiController
    {
        [Route("orders")]
        [System.Web.Http.AcceptVerbs("POST")]
        public IHttpActionResult orderPost()
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));

            IEnumerable<string> deviceValues;
            if (!Request.Headers.TryGetValues("device_id", out deviceValues))
                return Ok(new Error(400, "Bad request"));
            var deviceID = Request.Headers.GetValues("device_id").FirstOrDefault();

            IEnumerable<string> userValues;
            if (!Request.Headers.TryGetValues("user_id", out userValues))
                return Ok(new Error(400, "Bad request"));
            var userID = Request.Headers.GetValues("user_id").FirstOrDefault();

            DatabaseEntities db = new DatabaseEntities();
            var user = db.users.Where(x => x.Id.ToString() == userID).FirstOrDefault();
            if (user == null)
                return Ok(new Error(400, "Bad request"));

            var device = db.devices.Where(x => x.Id.ToString() == deviceID).FirstOrDefault();
            if (device == null)
                return Ok(new Error(400, "Bad request"));

            var order = new Database.order();
            order.device_id = device.Id;
            order.user_id = user.Id;
            order.date = DateTime.Now;
            db.orders.Add(order);
            db.SaveChanges();

            return Ok("Success!");
        }

        [Route("orders")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult orderGet()
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));

            IEnumerable<string> userValues;
            if (!Request.Headers.TryGetValues("user_id", out userValues))
                return Ok(new Error(400, "Bad request"));
            var userID = Request.Headers.GetValues("user_id").FirstOrDefault();

            DatabaseEntities db = new DatabaseEntities();
            List<OrderItem> orderList = new List<OrderItem>();
            foreach (Database.order order in db.orders)
            {
                orderList.Add(new OrderItem(order.Id, order.device_id, order.user_id, order.date));
            }

            return Ok(orderList);
        }

        [Route("orders")]
        [System.Web.Http.AcceptVerbs("DELETE")]
        public IHttpActionResult orderDelete()
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));

            IEnumerable<string> orderValues;
            if (!Request.Headers.TryGetValues("order_id", out orderValues))
                return Ok(new Error(400, "Bad request"));
            var orderID = Request.Headers.GetValues("order_id").FirstOrDefault();

            DatabaseEntities db = new DatabaseEntities();
            var order = db.orders.Where(x => x.Id.ToString() == orderID).FirstOrDefault();
            db.orders.Remove(order);
            db.SaveChanges();
            return Ok("Success!");
        }
    }
}
