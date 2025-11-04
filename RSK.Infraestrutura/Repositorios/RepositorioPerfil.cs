using Microsoft.EntityFrameworkCore;
using RSK.Dominio.Autorizacao.Entidades;
using RSK.Dominio.IRepositorios;

namespace RSK.Infraestrutura.Repositorios
{
    public class RepositorioPerfil<TContext> : IRepositorioPerfil where TContext : DbContext
    {
        protected readonly TContext _context;

        public RepositorioPerfil(TContext context)
        {
            _context = context;
        }

        public async Task<string?> ObterNomePerfilPorUsuarioIdAssincrono(long userId)
        {
            var nomePerfil = await (
                from up in _context.Set<UsuarioPerfil>()
                join p in _context.Set<Perfil>() on up.PerfilId equals p.Id
                where up.UsuarioId == userId
                select p.Nome
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();

            return nomePerfil;
        }

        public async Task<bool> VerificarPermissaoPerfilAssincrono(
            string nomePerfil,
            string controller,
            string action)
        {
            bool acessoPermitido = await _context.Set<PerfilPermissao>()
                .AsNoTracking()
                .Where(pp => pp.Perfil.Nome == nomePerfil &&
                             pp.Permissao.Controller == controller &&
                             pp.Permissao.Action == action &&
                             pp.Permitido == true)
                .AnyAsync();

            return acessoPermitido;
        }
    }
}
