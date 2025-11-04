using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RSK.Dominio.Entidades;
using RSK.Dominio.Notificacoes.Interfaces;
using RSK.Dominio.Servicos;
using RSK.Dominio.Autorizacao.Atributos;
using RSK.Dominio.Interfaces;

namespace RSK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AutorizarGenerico]
    public abstract class ApiCrudControllerBase<TEntity> : ApiConsultaControllerBase<TEntity>
        where TEntity : EntidadeBase
    {
        protected readonly IServicoCrudBase<TEntity> _servicoCrud;
        protected readonly INotificador _servicoMensagem;

        protected ApiCrudControllerBase(
            IServicoConsultaBase<TEntity> servicoConsulta,
            IServicoCrudBase<TEntity> servicoCrud,
            INotificador servicoMensagem
        ) : base(servicoConsulta)
        {
            _servicoCrud = servicoCrud;
            _servicoMensagem = servicoMensagem;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TEntity entidade)
        {
            var resultado = await _servicoCrud.AdicionarAssincrono(entidade);

            if (_servicoMensagem.PossuiErros())
                return BadRequest(new { mensagens = _servicoMensagem.ObterMensagens() });

            return Created(string.Empty, new
            {
                mensagens = new[] { new { Texto = $"{typeof(TEntity).Name} criada com sucesso.", Tipo = "Sucesso" } },
                dados = resultado
            });
        }

        [HttpPut("{id:long}")]
        public virtual async Task<IActionResult> Put(long id, [FromBody] TEntity entidade)
        {
            var existente = await _servicoCrud.ObterPorIdAssincrono(id);
            if (existente == null)
                return NotFound(new { mensagens = new[] { new { Texto = $"{typeof(TEntity).Name} não encontrada.", Tipo = "Erro" } } });

            var resultado = await _servicoCrud.AtualizarAssincrono(entidade);

            if (_servicoMensagem.PossuiErros())
                return BadRequest(new { mensagens = _servicoMensagem.ObterMensagens() });

            return Ok(new
            {
                mensagens = new[] { new { Texto = $"{typeof(TEntity).Name} atualizada com sucesso.", Tipo = "Sucesso" } },
                dados = resultado
            });
        }

        [HttpPatch("{id:long}")]
        public virtual async Task<IActionResult> Patch(long id, [FromBody] JsonPatchDocument<TEntity> patchDoc)
        {
            var resultado = await _servicoCrud.AtualizarParcialmenteAssincrono(id, patchDoc);

            if (_servicoMensagem.PossuiErros())
                return BadRequest(new { mensagens = _servicoMensagem.ObterMensagens() });

            if (resultado == null)
                return NotFound(new { mensagens = new[] { new { Texto = $"{typeof(TEntity).Name} não encontrada.", Tipo = "Erro" } } });

            return Ok(new
            {
                mensagens = new[] { new { Texto = $"{typeof(TEntity).Name} atualizada parcialmente com sucesso.", Tipo = "Sucesso" } },
                dados = resultado
            });
        }

        [HttpDelete("{id:long}")]
        public virtual async Task<IActionResult> Delete(long id)
        {
            var existente = await _servicoCrud.ObterPorIdAssincrono(id);
            if (existente == null)
                return NotFound(new { mensagens = new[] { new { Texto = $"{typeof(TEntity).Name} não encontrada.", Tipo = "Erro" } } });

            await _servicoCrud.RemoverAssincrono(id);

            if (_servicoMensagem.PossuiErros())
                return BadRequest(new { mensagens = _servicoMensagem.ObterMensagens() });

            return Ok(new
            {
                mensagens = new[] { new { Texto = $"{typeof(TEntity).Name} removida com sucesso.", Tipo = "Sucesso" } }
            });
        }
    }
}
