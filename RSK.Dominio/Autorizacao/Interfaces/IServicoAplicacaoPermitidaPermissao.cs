namespace RSK.Dominio.Autorizacao.Interfaces
{
    public interface IServicoAplicacaoPermitidaPermissao
    {
        Task<bool> VerificarPermissaoAssincrono(string integrationSecret, string controller, string acao);
    }
}
