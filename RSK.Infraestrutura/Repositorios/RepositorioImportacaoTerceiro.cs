using Microsoft.EntityFrameworkCore;
using RSK.Dominio.Entidades;
using RSK.Dominio.IRepositorios;

namespace RSK.Infraestrutura.Repositorios
{
    public class RepositorioImportacaoTerceiro<TEntity, TContext> : RepositorioBaseAssincrono<TEntity, TContext>, IRepositorioImportacaoTerceiro<TEntity>
        where TEntity : EntidadeBaseImportacaoTerceiro
        where TContext : DbContext
    {

        public RepositorioImportacaoTerceiro(TContext context) : base(context)
        {
        }

        public async Task<TEntity?> ObterPorIdTerceiroAssincrono(string idTerceiro)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.IdTerceiro == idTerceiro);
        }
    }
}
