using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Controllers
{
    [Authorize]
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
}
