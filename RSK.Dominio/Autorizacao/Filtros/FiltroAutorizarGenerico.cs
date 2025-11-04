using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RSK.Dominio.Autorizacao.Interfaces;
using RSK.Dominio.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;


namespace RSK.Dominio.Autorizacao.Filtros
{
    public class FiltroAutorizarGenerico : IAsyncActionFilter
    {
        private readonly IServicoVerificacaoAplicacao _servicoVerificacaoAplicacao;

        public FiltroAutorizarGenerico(IServicoVerificacaoAplicacao servicoVerificacaoAplicacao)
        {
            _servicoVerificacaoAplicacao = servicoVerificacaoAplicacao;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor != null &&
                (actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any() ||
                 actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()))
            {
                await next();
                return;
            }

            var httpContext = context.HttpContext;
            var request = httpContext.Request;

            // Usuário autenticado via Identity
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                await next();
                return;
            }

            // Secret header
            request.Headers.TryGetValue(Seguranca.IntegrationSecretHeader, out var secretValues);
            string? integrationSecret = secretValues.FirstOrDefault();

            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();

            var (Success, Result) = await _servicoVerificacaoAplicacao.VerificarAssincrono(
                integrationSecret,
                controllerName,
                actionName
            );

            if (Success)
            {
                await next();
                return;
            }

            context.Result = Result ?? new UnauthorizedResult();
        }
    }
}
