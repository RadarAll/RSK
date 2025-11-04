using RSK.Dominio.Autorizacao.Interfaces;
using RSK.Dominio.Constantes;

namespace RSK.API.Middlewares
{
    public class AplicacaoPermitidaPermissaoMiddleware
    {
        private readonly RequestDelegate _proximo;

        public AplicacaoPermitidaPermissaoMiddleware(
            RequestDelegate proximo,
            ILogger<AplicacaoPermitidaPermissaoMiddleware> logger)
        {
            _proximo = proximo;
        }

        public async Task InvokeAssincrono(HttpContext context, IServicoAplicacaoPermitidaPermissao servicoAutorizacao)
        {
            var request = context.Request;
            var endpoint = context.GetEndpoint();

            
            if (endpoint != null)
            {
                if (!request.Headers.TryGetValue(Seguranca.IntegrationSecretHeader, out var secretValues) ||
                    string.IsNullOrWhiteSpace(secretValues.FirstOrDefault()))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                string integrationSecret = secretValues.First()!;

                var controllerNome = context.GetRouteValue("controller")?.ToString();
                var actionNome = context.GetRouteValue("action")?.ToString();

                if (string.IsNullOrEmpty(controllerNome) || string.IsNullOrEmpty(actionNome))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return;
                }

                bool permitido = await servicoAutorizacao.VerificarPermissaoAssincrono(
                    integrationSecret,
                    controllerNome!,
                    actionNome!
                );

                if (!permitido)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }
            }

            await _proximo(context);
        }
    }
}
