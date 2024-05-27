using System.Net;
using System.Security.Principal;
using System.Text;
using Cloud4Feed.Application.Repository.Contract;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;


namespace Cloud4Feed.Application.Auth
{
    public class BasicAuthenticationFilter : ActionFilterAttribute
    {
        const string scheme = "Basic";

        readonly IUserRepository userRepository;

        public BasicAuthenticationFilter(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var anonymousAttr = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x.GetType() == typeof(AllowAnonymousAttribute));

            if (anonymousAttr != null)
                return;

            var authString = context.HttpContext.Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authString))
            {
                context.Result = new BadRequestObjectResult("Kullanıcı Bilgileri eksik");
                return;
            }

            var authToken = authString.Replace($"{scheme} ", "");
            string decodedAuthenticationToken = "";

            try
            {
                //Decode the string
                decodedAuthenticationToken = Encoding.UTF8.GetString(
                   Convert.FromBase64String(authToken));
            }
            catch (Exception ex)
            {
                context.Result = new BadRequestObjectResult("Kullanıcı Bilgileri eksik");
                return;
            }

            string[] mailPasswordArray = decodedAuthenticationToken.Split(':');

            if (mailPasswordArray.Length != 2)
            {
                context.Result = new BadRequestObjectResult("Kullanıcı Bilgileri eksik");
                return;
            }

            string email = mailPasswordArray[0];
            string password = mailPasswordArray[1];

            bool isAuthenticated = userRepository.Authenticate(email, password);

            if (!isAuthenticated)
            {
                context.Result = new BadRequestObjectResult("Kullanıcı bulunamadı");
                return;
            }

            var identity = new GenericIdentity(email);

            GenericPrincipal principal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = principal;

            if (context.HttpContext != null)
                context.HttpContext.User = principal;

        }
    }
}