using RSK.Dominio.Autorizacao.Entidades;
using RSK.Dominio.Interfaces;

namespace RSK.Dominio.Autorizacao.Interfaces
{
    public interface IServicoUsuario : IServicoCrudBase<UsuarioBase>
    {
        Task<UsuarioBase?> ObterPorEmail(string email);
        Task<bool> VerificarSenhaAsync(UsuarioBase usuario, string senhaDigitada);

    }
}
