using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AuthorizationServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Controllers
{
    [EnableCors("MyPolicy")]
    [Produces("application/json")]
    [Route("api")]
    public class AccauntController : Controller
    {
        private readonly IProfileRepository _Repository;
        public AccauntController(IServiceProvider services, IProfileRepository profileRepository)
        {
            _Repository = profileRepository;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> Registrarion(Profile request)
        {
            Profile user = new Profile
            {
                Name = request.Name,
                Login = request.Login,
                Password = request.Password
            };

            await _Repository.CreateProfile(user);

            return Ok(200);

        }

        [AllowAnonymous]
        [HttpPost("getprofileid")]
        public async Task<IActionResult> GetProfileId(Profile request)
        {

            var response = await _Repository.GetProfileIdAsync(request.Login, request.Password);

            return Ok(response);

        }

        [AllowAnonymous]
        [HttpGet("logintodoist")]
        public IActionResult Logintodoist()
        {
            var response = Getlogintodoist();
            if (response != null)
            {
                response.Close();
                return Ok(response.ResponseUri);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("gettokentodoist")]
        public IActionResult GetTokenTodoist(string code)
        {
            var response = Gettokentodoist(code);

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                return Ok(reader.ReadToEnd().ToString());
            }
        }

        public HttpWebResponse Getlogintodoist()
        {
            var request = WebRequest.Create("https://todoist.com/oauth/authorize?client_id=85b844dc19804f51a3bdac428bf61838&scope=data:read&state=secretstring");

            request.Method = "GET";

            HttpWebResponse respons = request.GetResponse() as HttpWebResponse;

            return respons;
        }

        public HttpWebResponse Gettokentodoist(string code)
        {
            var request = WebRequest.Create($"https://todoist.com/oauth/access_token?client_id=85b844dc19804f51a3bdac428bf61838&client_secret=8dcefb50a37c48b0b537ae32483fbada&code={code}");

            request.Method = "POST";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            return response;
        }
    }
}