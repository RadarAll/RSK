using RSK.Dominio.Entidades;
using RSK.Dominio.Interfaces;
using RSK.Dominio.IRepositorios;
using RSK.Dominio.Notificacoes.Interfaces;

namespace RSK.Dominio.Servicos
{
    public class ServicoConsultaBase<TEntity> : IServicoConsultaBase<TEntity>
        where TEntity : EntidadeBase
    {

        protected readonly IRepositorioBaseAssincrono<TEntity> _repositorio;
        protected readonly IServicoMensagem _mensagens;

        public ServicoConsultaBase(IRepositorioBaseAssincrono<TEntity> repositorio, IServicoMensagem mensagens)
        {
            _repositorio = repositorio;
            _mensagens = mensagens;
        }

        /// <summary>
        /// Validação básica antes de qualquer operação.
        /// Pode ser sobrescrito se precisar de regras específicas.
        /// </summary>
        protected virtual bool Validar(TEntity? entidade, string operacao = "")
        {
            if (entidade == null)
            {
                _mensagens.AdicionarErro($"Entidade {typeof(TEntity).Name} é nula na operação {operacao}.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Valida se a entidade existe antes de atualização ou remoção
        /// </summary>
        protected virtual async Task<TEntity?> ValidarExistenciaAsync(long id, string operacao = "")
        {
            var entidade = await _repositorio.ObterPorIdAssincrono(id);
            if (!Validar(entidade, operacao))
                return null;

            return entidade;
        }

        public virtual async Task<TEntity?> ObterPorIdAssincrono(long id)
        {
            return await _repositorio.ObterPorIdAssincrono(id);
        }

        public virtual async Task<List<TEntity>> ObterTodosComoListaAsync()
        {
            return await _repositorio.ObterTodosComoListaAsync();
        }

        public virtual Task<IEnumerable<TEntity>> ObterTodosAssincrono()
        {
            return _repositorio.ObterTodosAssincrono();
        }

        //public virtual Task<IEnumerable<TEntity>> BuscarAssincrono(Expression<Func<TEntity, bool>> predicado)
        //{
        //    return _repositorio.BuscarAssincrono(predicado);
        //}

        public IQueryable<TEntity> ObterIQueryable()
        {
            return _repositorio.Consulta;
        }
    }
}
