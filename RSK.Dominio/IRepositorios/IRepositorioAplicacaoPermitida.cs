using RSK.Dominio.Autorizacao.Entidades;

namespace RSK.Dominio.IRepositorios
{
    public interface IRepositorioAplicacaoPermitida
    {
        Task<AplicacaoPermitida?> ObterPorSecretHashAsync(string secretHash);
        Task<bool> VerificarPermissaoAsync(long aplicacaoId, string controller, string acao);
    }
}
