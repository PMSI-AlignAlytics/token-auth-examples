using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using WebApp.Repository;

namespace WebApp.Provider
{
    public class BearerAuthenticationProvider : IOAuthBearerAuthenticationProvider
    {

        private readonly IAppConfig _appConfig;

        /// <summary>
		/// Handles applying the authentication challenge to the response message.
		/// </summary>
		public Func<OAuthChallengeContext, Task> OnApplyChallenge
		{
			get;
			set;
		}

		/// <summary>
		/// Handles processing OAuth bearer token.
		/// </summary>
		public Func<OAuthRequestTokenContext, Task> OnRequestToken
		{
			get;
			set;
		}

		/// <summary>
		/// Handles validating the identity produced from an OAuth bearer token.
		/// </summary>
		public Func<OAuthValidateIdentityContext, Task> OnValidateIdentity
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationProvider" /> class
		/// </summary>
        public BearerAuthenticationProvider(IAppConfig appConfig)
		{
            _appConfig = appConfig;
            this.OnRequestToken = (OAuthRequestTokenContext context) =>
            {
                var idContext = new OAuthValidateIdentityContext(context.OwinContext, null, null);
              
                this.ValidateIdentity(idContext);
                return Task.FromResult<int>(0);
            };
			this.OnValidateIdentity = (OAuthValidateIdentityContext context) => Task.FromResult<object>(null);
            this.OnApplyChallenge = (OAuthChallengeContext context) => Task.FromResult<object>(null);
		}

        /// <summary>
		/// Handles applying the authentication challenge to the response message.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public Task ApplyChallenge(OAuthChallengeContext context)
		{
			return this.OnApplyChallenge(context);
		}

		/// <summary>
		/// Handles processing OAuth bearer token.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual Task RequestToken(OAuthRequestTokenContext context)
		{
			return this.OnRequestToken(context);
		}

		/// <summary>
		/// Handles validating the identity produced from an OAuth bearer token.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual Task ValidateIdentity(OAuthValidateIdentityContext context)
		{
            if (context.Request.Headers.ContainsKey("Authorization"))
            {

                var jwt = context.Request.Headers["Authorization"].Replace("Bearer ", string.Empty);

                var handler = new JwtSecurityTokenHandler();

                var token = new System.IdentityModel.Tokens.JwtSecurityToken(jwt);
               

                var claimIdentity = new ClaimsIdentity(token.Claims, DefaultAuthenticationTypes.ExternalBearer);
                string DefaultSymmetricKeyEncoded_256 = Convert.ToBase64String(Encoding.UTF8.GetBytes(_appConfig.Key));
                byte[] DefaultSymmetricKeyBytes_256 = Convert.FromBase64String(DefaultSymmetricKeyEncoded_256);
                var key = new InMemorySymmetricSecurityKey(DefaultSymmetricKeyBytes_256);
                var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
                var prov = new SymmetricSignatureProvider(key, SecurityAlgorithms.HmacSha256Signature);
               
                var param = new TokenValidationParameters();
                param.ValidateAudience = false;
                param.ValidateIssuer = false;
                param.IssuerSigningKey = key;
                SecurityToken t = null;
                var identity = handler.ValidateToken(token.RawData, param, out t);

                var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                context.Response.Context.Authentication.User = claimPrincipal;
                context.Validated(claimIdentity);
            }
            return this.OnValidateIdentity(context);
		}

    }
}