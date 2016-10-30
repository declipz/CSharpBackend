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
    public class CartController : ApiController
    {
        [Route("cart")]
        [System.Web.Http.AcceptVerbs("POST")]
        public IHttpActionResult cartPost()
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

            var cart = new Database.cart();
            cart.device_id = device.Id;
            cart.user_id = user.Id;
            db.carts.Add(cart);
            db.SaveChanges();

            return Ok("Success!");
        }

        [Route("cart")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult cartGet()
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
            List<CartItem> cartList = new List<CartItem>();
            foreach(Database.cart cart in db.carts)
            {
                cartList.Add(new CartItem(cart.Id, cart.device_id, cart.user_id));
            }

            return Ok(cartList);
        }

        [Route("cart")]
        [System.Web.Http.AcceptVerbs("DELETE")]
        public IHttpActionResult cartDelete()
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));

            IEnumerable<string> cartValues;
            if (!Request.Headers.TryGetValues("cart_id", out cartValues))
                return Ok(new Error(400, "Bad request"));
            var cartID = Request.Headers.GetValues("cart_id").FirstOrDefault();

            DatabaseEntities db = new DatabaseEntities();
            var cart = db.carts.Where(x => x.Id.ToString() == cartID).FirstOrDefault();
            db.carts.Remove(cart);
            db.SaveChanges();
            return Ok("Success!");
        }
    }
}
