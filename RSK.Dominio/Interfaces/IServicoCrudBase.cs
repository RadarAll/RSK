using Microsoft.AspNetCore.JsonPatch;
using RSK.Dominio.Entidades;

namespace RSK.Dominio.Interfaces
{

    public interface IServicoCrudBase<TEntity> : IServicoConsultaBase<TEntity> where TEntity : EntidadeBase
    {
        Task<TEntity?> AdicionarAssincrono(TEntity entidade);
        Task<TEntity?> AtualizarAssincrono(TEntity entidade);
        Task<TEntity?> AtualizarParcialmenteAssincrono(long id, JsonPatchDocument<TEntity> patchDoc);
        Task RemoverAssincrono(long id);
    }

}
