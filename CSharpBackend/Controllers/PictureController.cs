using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CSharpBackend.Models;
using CSharpBackend.Database;

namespace CSharpBackend.Controllers
{
    public class PictureController : ApiController
    {
        [System.Web.Http.Route("pictures")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult pictureGet()
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));
            DatabaseEntities db = new DatabaseEntities();

            List<Picture> pictures = new List<Models.Picture>();
            foreach(Database.picture picture in db.pictures)
            {
                pictures.Add(new Picture(picture.Id, picture.device_id, picture.url));
            }
            IEnumerable<string> deviceValues;
            if (Request.Headers.TryGetValues("device_id", out deviceValues))
            {
                var deviceID = Request.Headers.GetValues("device_id").FirstOrDefault();
                pictures = pictures.Where(x => x.device_id.ToString() == deviceID).ToList<Picture>();
            }
            return Ok(pictures);
        }
    }
}
