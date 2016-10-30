using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Mail;
using CSharpBackend.Models;
using CSharpBackend.Database;

namespace CSharpBackend.Controllers
{
    public class RegisterController : ApiController
    {
        [Route("register")]
        [System.Web.Http.AcceptVerbs("POST")]
        public IHttpActionResult Register(UserBlank blank)
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));
            if (!ModelState.IsValid)
                return Ok(new Error(400, "Bad request"));
            if (!isEmail(blank.email))
                return Ok(new Error(400, "Bad request"));
            DatabaseEntities db = new DatabaseEntities();
            foreach (Database.user dbUser in db.users)
            {
                if (blank.email.Trim() == dbUser.email.Trim())
                    return Ok(new Error(400, "Bad request."));
            }
            Database.user user = new Database.user();
            user.email = blank.email.Trim();
            user.password = blank.password.Trim();
            db.users.Add(user);
            db.SaveChanges();
            return Ok("Success");
        }

        public bool isEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
