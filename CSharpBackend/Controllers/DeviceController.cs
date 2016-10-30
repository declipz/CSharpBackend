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
    public class DeviceController : ApiController
    {
        [Route("platforms")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult platformsGet()
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));

            DatabaseEntities db = new DatabaseEntities();
            List<Platform> platforms = new List<Platform>();
            foreach(Database.operating_systems platform in db.operating_systems)
            {
                platforms.Add(new Platform(platform.Id, platform.platform_name.Trim()));
            }
            return Ok(platforms);
        }

        [Route("brands")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult brandsGet()
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));

            IEnumerable<string> platformValues;
            if (!Request.Headers.TryGetValues("platform_id", out platformValues))
                return Ok(new Error(400, "Bad request"));
            var platformID = Request.Headers.GetValues("platform_id").FirstOrDefault();

            DatabaseEntities db = new DatabaseEntities();
            List<Brand> brands = new List<Brand>();
            foreach(Database.brand brand in db.brands.Where(x => x.platform_id.ToString().Trim() == platformID.Trim()))
            {
                brands.Add(new Brand(brand.Id, brand.platform_id, brand.brand_name.Trim()));
            }
            return Ok(brands);
        }

        [Route("devices")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult devicesGet()
        {
            IEnumerable<string> authkeyValues;
            if (!Request.Headers.TryGetValues("authkey", out authkeyValues))
                return Ok(new Error(400, "Bad request"));
            var authkey = Request.Headers.GetValues("authkey").FirstOrDefault();
            if (authkey != Constants.AUTH_KEY)
                return Ok(new Error(403, "Forbidden"));

            DatabaseEntities db = new DatabaseEntities();
            List<Device> devices = new List<Device>();
            foreach (Database.device device in db.devices)
            {
                devices.Add(new Models.Device(device.Id, device.brand_id, device.brand.platform_id,
                    device.name.Trim(), device.cpu.Trim(), device.ram.Trim(),
                    device.price, device.description.Trim()));
            }

            IEnumerable<string> brandValues;
            if (Request.Headers.TryGetValues("brand_id", out brandValues))
            {
                var brandID = Request.Headers.GetValues("brand_id").FirstOrDefault();
                return Ok(devices.Where(x => x.brand_id.ToString() == brandID).ToList());
            }
            
            IEnumerable<string> deviceValues;
            if (Request.Headers.TryGetValues("device_id", out deviceValues))
            {
                var deviceID = Request.Headers.GetValues("device_id").FirstOrDefault();
                return Ok(devices.Where(x => x.id.ToString() == deviceID).FirstOrDefault());
            }

            return Ok(devices);
        }
    }
}
