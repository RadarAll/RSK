using Microsoft.AspNetCore.JsonPatch;
using RSK.Dominio.Entidades;
using RSK.Dominio.Interfaces;
using RSK.Dominio.IRepositorios;
using RSK.Dominio.Notificacoes.Interfaces;

namespace RSK.Dominio.Servicos
{
    public class ServicoCrudBase<TEntity> : ServicoConsultaBase<TEntity>, IServicoCrudBase<TEntity>
        where TEntity : EntidadeBase
    {
        protected readonly ITransacao _transacao;


        public ServicoCrudBase(
            IRepositorioBaseAssincrono<TEntity> repositorio,
            IServicoMensagem mensagens,
            ITransacao transacao
        ) : base(repositorio, mensagens)
        {
            _transacao = transacao;
        }

        public virtual async Task<TEntity?> AdicionarAssincrono(TEntity entidade)
        {
            if (!Validar(entidade, "Adicionar"))
                return null;

            try
            {
                await _repositorio.AdicionarAssincrono(entidade);
                await _transacao.CommitAssincrono();
                _mensagens.Adicionar($"{typeof(TEntity).Name} adicionada com sucesso!");
                return entidade;
            }
            catch (Exception ex)
            {
                _mensagens.AdicionarErro($"Erro ao adicionar {typeof(TEntity).Name}: {ex.Message}");
                return null;
            }
        }

        public virtual async Task<TEntity?> AtualizarAssincrono(TEntity entidade)
        {
            if (!Validar(entidade, "Atualizar"))
                return null;

            try
            {
                entidade.AtualizarDataAlteracao();
                _repositorio.AtualizarAssincrono(entidade);
                await _transacao.CommitAssincrono();
                _mensagens.Adicionar($"{typeof(TEntity).Name} atualizada com sucesso!");
                return entidade;
            }
            catch (Exception ex)
            {
                _mensagens.AdicionarErro($"Erro ao atualizar {typeof(TEntity).Name}: {ex.Message}");
                return null;
            }
        }


        public virtual async Task<TEntity?> AtualizarParcialmenteAssincrono(long id, JsonPatchDocument<TEntity> patchDoc)
        {
            var entidade = await _repositorio.ObterPorIdAssincrono(id);
            if (entidade == null)
            {
                _mensagens.AdicionarErro($"{typeof(TEntity).Name} não encontrada.");
                return null;
            }

            patchDoc.ApplyTo(entidade);

            entidade.AtualizarDataAlteracao();
            _repositorio.AtualizarAssincrono(entidade);
            await _transacao.CommitAssincrono();

            _mensagens.Adicionar($"{typeof(TEntity).Name} atualizada parcialmente com sucesso.");
            return entidade;
        }


        public virtual async Task RemoverAssincrono(long id)
        {
            var entidade = await ValidarExistenciaAsync(id, "Remover");
            if (entidade == null) 
            {
                return;
            }

            try
            {
                await _repositorio.DeletarAssincrono(id);
                await _transacao.CommitAssincrono();
                _mensagens.Adicionar($"{typeof(TEntity).Name} removida com sucesso!");
            }
            catch (Exception ex)
            {
                _mensagens.AdicionarErro($"Erro ao remover {typeof(TEntity).Name}: {ex.Message}");
            }
        }
    }
}
