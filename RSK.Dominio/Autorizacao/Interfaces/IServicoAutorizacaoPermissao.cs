using RSK.Dominio.Autorizacao.Entidades;

namespace RSK.Dominio.Autorizacao.Interfaces
{
    public interface IServicoAutorizacaoPermissao
    {
        Task<bool> VerificarAcessoPorPerfilAssincrono(long userId, Permissao permissao);
    }
}
