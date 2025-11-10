using RSK.Dominio.Autorizacao.Interfaces;
using RSK.Dominio.IRepositorios;

namespace RSK.Dominio.Autorizacao.Servicos
{
    public class ServicoAplicacaoPermitidaPermissao : IServicoAplicacaoPermitidaPermissao
    {
        private readonly IRepositorioAplicacaoPermitida _repositorio;
        private readonly IHasher _hasher;

        public ServicoAplicacaoPermitidaPermissao(
            IRepositorioAplicacaoPermitida repositorio,
            IHasher hasher)
        {
            _repositorio = repositorio;
            _hasher = hasher;
        }

        public async Task<bool> VerificarPermissaoAssincrono(
            string integrationSecret,
            string controller,
            string acao)
        {
            //string secretHash = _hasher.Hash(integrationSecret);

            var aplicacao = await _repositorio.ObterPorSecretHashAsync(integrationSecret);

            if (aplicacao == null || !aplicacao.Ativa)
            {
                return false;
            }

            string controllerNormalizado = controller.ToUpperInvariant();
            string acaoNormalizada = acao.ToUpperInvariant();

            bool temPermissao = await _repositorio.VerificarPermissaoAsync(
                aplicacao.Id,
                controllerNormalizado,
                acaoNormalizada);

            return temPermissao;
        }
    }
}
