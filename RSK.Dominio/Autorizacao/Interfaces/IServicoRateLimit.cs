namespace RSK.Dominio.Autorizacao.Interfaces
{
    public interface IServicoRateLimit
    {
        Task<bool> PermitirEContarAssincrono(long clientId);
        Task<(int Limite, int Usado, string PlanoNome)> ObterStatusUsoAssincrono(long clientId);
    }
}
