namespace RSK.Dominio.IRepositorios
{
    public interface IRepositorioPerfil
    {
        Task<string?> ObterNomePerfilPorUsuarioIdAssincrono(long userId);
        Task<bool> VerificarPermissaoPerfilAssincrono(string nomePerfil, string controller, string action);

    }
}
