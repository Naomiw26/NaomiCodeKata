using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RPGCombatKata.Api.Characters;
using System.Net;

namespace RPGCombatKata.Api.Service.Exceptions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            if (context.Exception is EntityNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                
            }
            if (context.Exception is CharacterIsDeadWhenHealedException)
            {
                statusCode = HttpStatusCode.Forbidden;
               
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.Result = new JsonResult(new
            {
                error = new[] { context.Exception.Message },
                stackTrace = context.Exception.StackTrace
            });
            
        }
    }
}
