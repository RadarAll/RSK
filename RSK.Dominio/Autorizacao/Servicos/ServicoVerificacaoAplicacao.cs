using Microsoft.AspNetCore.Mvc;
using RSK.Dominio.Autorizacao.Interfaces;

namespace RSK.Dominio.Autorizacao.Servicos
{
    public class ServicoVerificacaoAplicacao : IServicoVerificacaoAplicacao
    {
        private readonly IServicoAplicacaoPermitidaPermissao _servicoAutorizacao;

        public ServicoVerificacaoAplicacao(IServicoAplicacaoPermitidaPermissao servicoAutorizacao)
        {
            _servicoAutorizacao = servicoAutorizacao;
        }

        public async Task<(bool Success, IActionResult? Result)> VerificarAssincrono(
            string? secret,
            string? controllerName,
            string? actionName)
        {
            // 1. Validação de Argumentos (Secret)
            if (string.IsNullOrEmpty(secret))
            {
                // 401: Secret ausente ou inválido
                return (false, new UnauthorizedResult());
            }

            // 2. Validação de Argumentos (Rota)
            if (string.IsNullOrEmpty(controllerName) || string.IsNullOrEmpty(actionName))
            {
                // 400: Informação de rota ausente
                return (false, new BadRequestObjectResult(new { Message = "Não foi possível encontrar a rota chamada" }));
            }

            // 3. Verificar Permissão (Lógica no serviço de domínio)
            bool permitido = await _servicoAutorizacao.VerificarPermissaoAssincrono(
                secret,
                controllerName!,
                actionName!
            );

            if (permitido)
            {
                return (true, null);
            }
            else
            {
                return (false, new ForbidResult());
            }
        }
    }
}
