using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApp.Models;
using WebApp.Repository;

namespace WebApp.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;

        public AccountController()
        {
            _repo = new AuthRepository();
        }

        public class Body
        {
            public string grant_type { get; set; }
            public string username { get; set; }
            public string password { get; set; }
        }
        public class JsonContent : HttpContent
        {
            private readonly JToken _value;

            public JsonContent(JToken value)
            {
                _value = value;
                Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            protected override Task SerializeToStreamAsync(Stream stream,
                TransportContext context)
            {
                var jw = new JsonTextWriter(new StreamWriter(stream))
                {
                    Formatting = Formatting.Indented
                };
                _value.WriteTo(jw);
                jw.Flush();
                return Task.FromResult<object>(null);
            }

            protected override bool TryComputeLength(out long length)
            {
                length = -1;
                return false;
            }
        }

        [AllowAnonymous]
        [Route("token")]
        [HttpPost]
        public async Task<HttpResponseMessage> Token([FromBody] Body body)
        {
            HttpResponseMessage message = null;
            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser user = await _repo.FindUser(body.username, body.password);

                if (user == null)
                {
                    message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    return message;
                }
            }

            var token = new System.IdentityModel.Tokens.JwtSecurityToken("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6ImFkbWluQHNhbGVzYWxpZ24uY29tIiwic3ViIjoiMTIzNDU2Nzg5MCIsImlzcyI6Im9tZWdhIiwiZXhwIjoxNDE3MTkxMDA4LCJuYmYiOjE0MTcxODc0MDh9.eD4sShue3-YenXCmNXlNGuRHOUuKRhvKZAPxzn2X9OM");
            

            message = new HttpResponseMessage(HttpStatusCode.OK);
            JToken json = JObject.Parse("{ 'access_token': '" + token.RawData + "', 'token_type': 'bearer', 'expires_in': 86399 }");
            message.Content = new JsonContent(json);
            
            return message;

        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
