using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        // GET api/auth/v1
        [HttpGet]
        public string Get()
        {
            return DateTime.UtcNow.ToString("F") + " API is up and Running";
        }

        [HttpGet("[action]")]
        public JsonResult TestLog()
        {
            try
            {
                Log.Instance.Debug("We're going to throw an exception now.");
                Log.Instance.Warn("It's gonna happen!!");
                return new JsonResult("Hello World");
            }
            catch (ApplicationException ae)
            {
                Log.Instance.ErrorException("Error doing something...", ae);
                return new JsonResult("Error");
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/v1/token
        [HttpPost]
        public JsonResult Post([FromBody] string username)
        {
            var claims = new[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.UserData, "Hello World") };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(username));
            var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken
            (
                issuer: "cspro.com",
                audience: "cspro.com",
                claims: claims,
                signingCredentials: signInCredentials,
                expires: DateTime.UtcNow.AddMinutes(2)
            );
            return Json(new { token });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
