using Microsoft.AspNetCore.Mvc.Filters;
using RSK.Dominio.Autorizacao.Interfaces;
using RSK.Dominio.Constantes;

namespace RSK.Dominio.Autorizacao.Filtros
{
    public class FiltroAplicacaoPermitidaPermissao : IAsyncActionFilter
    {
        private readonly IServicoVerificacaoAplicacao _servicoVerificacaoAplicacao;

        public FiltroAplicacaoPermitidaPermissao(IServicoVerificacaoAplicacao servicoVerificacaoAplicacao)
        {
            _servicoVerificacaoAplicacao = servicoVerificacaoAplicacao;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            // 1. Obter Secret
            string? integrationSecret = null;
            if (request.Headers.TryGetValue(Seguranca.IntegrationSecretHeader, out var secretValues) &&
                !string.IsNullOrWhiteSpace(secretValues.FirstOrDefault()))
            {
                integrationSecret = secretValues.First()!;
            }

            // 2. Obter Controller e Ação (Via RouteData, específico do Filtro)
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
            }
            else
            {
                context.Result = Result;
            }
        }
    }
}
