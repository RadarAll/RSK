using Microsoft.EntityFrameworkCore;
using RSK.Dominio.Autorizacao.Entidades;
using RSK.Dominio.IRepositorios;

namespace RSK.Infraestrutura.Repositorios
{
    public class RepositorioAplicacaoPermitida<TContext> : RepositorioBaseAssincrono<AplicacaoPermitida, TContext>, IRepositorioAplicacaoPermitida
    where TContext : DbContext
    {
        public RepositorioAplicacaoPermitida(TContext context) : base(context)
        {
        }

        public async Task<AplicacaoPermitida?> ObterPorSecretHashAsync(string secretHash)
        {
            return await Consulta
                .Where(a => a.IntegrationSecretHash == secretHash && a.Ativa)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> VerificarPermissaoAsync(long aplicacaoId, string controller, string acao)
        {
            string controllerUpper = controller.ToUpperInvariant();
            string acaoUpper = acao.ToUpperInvariant();

            return await _context.Set<AplicacaoPermitidaPermissao>()
                .AsNoTracking()
                .AnyAsync(p =>
                    p.AplicacaoPermitidaId == aplicacaoId &&
                    p.Controller.ToUpper() == controllerUpper &&
                    p.Acao.ToUpper() == acaoUpper);
        }
    }
}
