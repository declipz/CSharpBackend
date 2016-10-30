using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using CSharpBackend.Database;
using CSharpBackend.Models;

namespace CSharpBackend.Controllers
{
    public class AuthController : ApiController
    {
        [System.Web.Http.Route("auth")]
        [System.Web.Http.AcceptVerbs("POST")]
        public IHttpActionResult Auth(UserBlank blank)
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));
            if (!ModelState.IsValid)
                return Ok(new Error(400, "Bad request"));

            DatabaseEntities db = new DatabaseEntities();
            var usersQuery = db.users;
            foreach (Database.user user in db.users)
            {
                if (blank.email.Trim() == user.email.Trim() && blank.password.Trim() == user.password.Trim())
                    return Ok(new User(user.Id, user.email.Trim(), user.password.Trim()));
            }
            return Ok(new Error(400, "Bad request"));
        }
    }
}