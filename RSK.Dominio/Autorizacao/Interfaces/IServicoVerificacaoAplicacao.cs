using Microsoft.AspNetCore.Mvc;

namespace RSK.Dominio.Autorizacao.Interfaces
{
    public interface IServicoVerificacaoAplicacao
    {
        Task<(bool Success, IActionResult? Result)> VerificarAssincrono(
            string? secret,
            string? controllerName,
            string? actionName
        );
    }
}
