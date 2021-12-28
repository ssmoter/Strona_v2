using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Strona_v2.Server.TokenAuthentication;

namespace Strona_v2.Server.Filtres
{
    public class TokenAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenManager = (ITokenManager)context.HttpContext.RequestServices.GetService(typeof(ITokenManager));

            bool result = true;
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                result = false;

            string token = string.Empty;

            if (result)
            {
                token = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;
                bool CheckToken = await tokenManager.VerifyToken(token);
                if (!CheckToken)
                {
                    result = false;
                }
            }
            if (!result)
            {
                context.ModelState.AddModelError("Unauthorization", "You are not Authorized.");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }


        }
    }
}
