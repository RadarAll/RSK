using RSK.Dominio.Entidades;

namespace RSK.Dominio.Interfaces
{
    public interface IServicoImportacaoTerceiro<TEntity> where TEntity : EntidadeBaseImportacaoTerceiro
    {
        Task<TEntity?> ObterPorIdTerceiroAssincrono(string idTerceiro);
    }
}
