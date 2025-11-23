using RSK.Dominio.Autorizacao.Entidades;
using RSK.Dominio.IRepositorios;

namespace RSK.Dominio.Autorizacao.Helpers
{
    public static class AplicacaoIntegradaHelper
    {
        /// <summary>
        /// Recupera uma aplicação integrada por ID.
        /// </summary>
        public static async Task<AplicacaoIntegrada?> ObterPorIdAsync(IRepositorioBaseAssincrono<AplicacaoIntegrada> repositorio, long id)
        {
            if (repositorio == null)
            {
                throw new ArgumentNullException(nameof(repositorio));
            }

            return await repositorio.ObterPorIdAssincrono(id);
        }
    }
}
