using RSK.Dominio.Entidades;
using RSK.Dominio.Notificacoes.Interfaces;

namespace RSK.Dominio.Interfaces
{
    public interface IServicoConsultaBase<TEntity> where TEntity : EntidadeBase
    {
        Task<TEntity?> ObterPorIdAssincrono(long id);
        Task<IEnumerable<TEntity>> ObterTodosAssincrono();
        Task<List<TEntity>> ObterTodosComoListaAsync();
        //Task<IEnumerable<TEntity>> BuscarAssincrono(Expression<Func<TEntity, bool>> predicado);
        IQueryable<TEntity> ObterIQueryable();
    }
}
