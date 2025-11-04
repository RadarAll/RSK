using Microsoft.AspNetCore.Http;
using RSK.Dominio.Autorizacao.Entidades;
using RSK.Dominio.Autorizacao.Interfaces;
using RSK.Dominio.IRepositorios;
using System.Security.Claims;


namespace RSK.Dominio.Autorizacao.Servicos
{
    public class ServicoAutorizacaoPermissao : IServicoAutorizacaoPermissao
    {
        private readonly IRepositorioPerfil _repositorioPerfil;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServicoAutorizacaoPermissao(
            IRepositorioPerfil repositorioPerfil,
            IHttpContextAccessor httpContextAccessor)
        {
            _repositorioPerfil = repositorioPerfil;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> VerificarAcessoPorPerfilAssincrono(long userId, Permissao permissao)
        {
            if (userId <= 0)
            {
                return false;
            }

            var nomePerfil = await _repositorioPerfil.ObterNomePerfilPorUsuarioIdAssincrono(userId);

            if (string.IsNullOrEmpty(nomePerfil))
            {
                return false;
            }

            bool acessoPermitido = await _repositorioPerfil.VerificarPermissaoPerfilAssincrono(
                nomePerfil,
                permissao.Controller,
                permissao.Action);

            return acessoPermitido;
        }
    }
}
