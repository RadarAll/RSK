using RSK.Dominio.Entidades;
using RSK.Dominio.Interfaces;
using RSK.Dominio.IRepositorios;
using RSK.Dominio.Notificacoes.Entidades;
using RSK.Dominio.Notificacoes.Interfaces;

namespace RSK.Dominio.Servicos
{
    public class ServicoImportacaoTerceiro<TEntity> : IServicoImportacaoTerceiro<TEntity> where TEntity : EntidadeBaseImportacaoTerceiro
    {
        protected readonly IRepositorioImportacaoTerceiro<TEntity> _repositorio;
        protected readonly IServicoMensagem _mensagens;

        public ServicoImportacaoTerceiro(
            IRepositorioImportacaoTerceiro<TEntity> repositorio,
            IServicoMensagem mensagens)
        {
            _repositorio = repositorio;
            _mensagens = mensagens;
        }

        public async Task<TEntity?> ObterPorIdTerceiroAssincrono(string idTerceiro)
        {
            var entidade = await _repositorio.ObterPorIdTerceiroAssincrono(idTerceiro);

            if (entidade == null)
            {

                var nomeEntidade = typeof(TEntity).Name;
                _mensagens.Adicionar(
                    $"{nomeEntidade} com ID de Terceiro '{idTerceiro}' não encontrado. A sincronização deste dado mestre deve ser executada antes.",
                    TipoMensagem.Erro);

                return null;
            }

            return entidade;
        }
    }
}