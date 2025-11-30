using System.Linq.Expressions;

namespace RSK.Dominio.IRepositorios
{
    public interface IRepositorioBaseAssincrono<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Consulta { get; }
        Task<IEnumerable<TEntity>> ObterTodosAssincrono();
        Task<List<TEntity>> ObterTodosComoListaAsync();
        Task<TEntity?> ObterPorIdAssincrono(object id);
        Task<IEnumerable<TEntity>> BuscarAssincrono(Expression<Func<TEntity, bool>> predicate);
        Task AdicionarAssincrono(TEntity entity);
        void AtualizarAssincrono(TEntity entity);
        Task DeletarAssincrono(object id);
        Task<int> SalvarAlteracoesAssincrono();


        Task BulkAdicionarAssincrono(IEnumerable<TEntity> entidades);
        Task BulkAtualizarAssincrono(IEnumerable<TEntity> entidades);
    }
}
