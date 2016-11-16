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
    public class NewsController : ApiController
    {
        [Route("news")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult newsGet()
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));

            DatabaseEntities db = new DatabaseEntities();
            List<NewsItem> newsList = new List<NewsItem>();

            foreach (Database.news newsItem in db.news)
            {
                newsList.Add(new NewsItem(newsItem.Id, newsItem.title.Trim(), newsItem.description.Trim(), newsItem.date, newsItem.device_id));
            }

            return Ok(newsList);
        }
    }
}
