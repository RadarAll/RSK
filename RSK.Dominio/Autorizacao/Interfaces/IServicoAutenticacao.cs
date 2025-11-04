namespace RSK.Dominio.Autorizacao.Interfaces
{
    public interface IServicoAutenticacao
    {
        Task<string?> AutenticarAsync(string email, string senha);
    }
}
