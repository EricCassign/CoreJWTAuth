using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    //[ApiController]
    public class AuthController : Controller
    {
        public readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
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
                Log.Instance.Error("Error doing something...", ae);
                return new JsonResult("Error");
            }
        }
         

        [HttpPost("[action]")]
        public JsonResult GenerateToken([FromBody] string o)
        {
            try
            {
                var site = _configuration["AuthSettings:SiteName"];
                var siteKey = _configuration["AuthSettings:SecurityKey"];
                var userData = new { Username = o, Time = DateTime.UtcNow };

                var claims = new[] { new Claim(ClaimTypes.Name, o), new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(userData)) };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(siteKey));
                var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var token = new JwtSecurityToken
                (
                    issuer: site,
                    audience: site,
                    claims: claims,
                    signingCredentials: signInCredentials,
                    expires: DateTime.UtcNow.AddMinutes(2)
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Json(new { Success = true, Message = "Generated", Raw = token, String = tokenString });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Message = "Error Occured :" + e.Message, Code = 500 });
            }
        }

        // PUT api/values/5
        [HttpPut("[action]")]
        public JsonResult Put([FromBody] string value)
        {
            return Json(new { Success = true, Message = "Value Put :" + value, });
        }
         
    }
}
