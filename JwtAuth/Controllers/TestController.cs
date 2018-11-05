using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Controllers
{
    [Route("api/test")]
    public class TestController : Controller
    {
        [HttpPost("[action]")]
        public JsonResult TestPost([FromBody] TestRequest request)

        {
            var u = request.Username;
            return Json(new { request });
        }

    }

    public class TestRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
