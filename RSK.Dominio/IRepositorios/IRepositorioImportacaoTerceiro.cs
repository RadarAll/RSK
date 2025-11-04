using RSK.Dominio.Entidades;


namespace RSK.Dominio.IRepositorios
{
    public interface IRepositorioImportacaoTerceiro<TEntity> : IRepositorioBaseAssincrono<TEntity> where TEntity : EntidadeBaseImportacaoTerceiro
    {
        Task<TEntity?> ObterPorIdTerceiroAssincrono(string idTerceiro);
    }
}
