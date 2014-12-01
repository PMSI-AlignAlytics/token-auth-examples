using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;

namespace WebApp.Handler
{
    public class JwtTokenHandler : JwtSecurityTokenHandler
    {
        public JwtTokenHandler()
        {
           
        }

        public override SecurityToken ReadToken(string tokenString)
        {
            var secutrityToken = base.ReadToken(tokenString);
            return secutrityToken;
        }

        protected override JwtSecurityToken ValidateSignature(string token, TokenValidationParameters validationParameters)
        {

            JwtSecurityToken jwt = null;
            jwt = base.ValidateSignature(token, validationParameters);
            return jwt;
        }

        public override System.Collections.ObjectModel.ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            return base.ValidateToken(token);
        }

        public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            ClaimsPrincipal p = null;
            p = base.ValidateToken(securityToken, validationParameters, out validatedToken);
            return p;
        }


        public override bool CanReadToken(string tokenString)
        {
            return base.CanReadToken(tokenString);
        }

    }
}