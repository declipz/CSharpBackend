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
    public class SearchController : ApiController
    {
        [Route("search")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult searchGet()
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));

            DatabaseEntities db = new DatabaseEntities();
            List<Device> devices = new List<Device>();
            IEnumerable<string> nameValues;
            foreach(Database.device device in db.devices)
            {
                if(Request.Headers.TryGetValues("name", out nameValues))
                {
                    var name = Request.Headers.GetValues("name").FirstOrDefault();
                    if (!device.brand.brand_name.ToLower().Contains(name.ToLower()) && 
                        !device.name.ToLower().Contains(name.ToLower()))
                        continue;
                }
                devices.Add(new Device(device.Id, device.brand_id, device.brand.platform_id, 
                    device.name.Trim(), device.cpu.Trim(), device.ram.Trim(), 
                    device.price, device.description.Trim(), device.screen_size.Trim(),
                    device.battery_capacity.Trim(), device.weight.Trim(), device.flash_memory.Trim()));
            }

            IEnumerable<string> platformValues;
            if(Request.Headers.TryGetValues("platform_id", out platformValues))
            {
                var platformID = Request.Headers.GetValues("platform_id").FirstOrDefault();
                foreach(Device device in devices.ToList())
                {
                    if (device.platform_id.ToString().Trim() != platformID.Trim())
                        devices.Remove(device);
                }
            }

            IEnumerable<string> brandValues;
            if (Request.Headers.TryGetValues("brand_id", out brandValues))
            {
                var brandID = Request.Headers.GetValues("brand_id").FirstOrDefault();
                foreach (Device device in devices.ToList())
                {
                    if (device.brand_id.ToString().Trim() != brandID)
                        devices.Remove(device);
                }
            }

            return Ok(devices);
        }
    }
}
