using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RSK.Dominio.Autorizacao.Atributos;
using RSK.Dominio.Entidades;
using RSK.Dominio.Interfaces;
using RSK.Dominio.Servicos;

namespace RSK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AutorizarGenerico]
    public abstract class ApiConsultaControllerBase<TEntity> : ControllerBase
        where TEntity : EntidadeBase
    {
        protected readonly IServicoConsultaBase<TEntity> _servicoConsulta;

        protected ApiConsultaControllerBase(IServicoConsultaBase<TEntity> servicoConsulta)
        {
            _servicoConsulta = servicoConsulta;
        }

        /// <summary>
        /// Retorna os dados da entidade com suporte a OData (filtro, ordenação, paginação).
        /// Exemplo: GET /api/entidade?$filter=Ativo eq true&$orderby=Nome asc
        /// </summary>
        [HttpGet("OData")]
        [EnableQuery]
        public virtual IActionResult GetOData()
        {
            var query = _servicoConsulta.ObterIQueryable();
            return Ok(query);
        }

        /// <summary>
        /// Retorna todos os registros da entidade.
        /// </summary>
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var lista = await _servicoConsulta.ObterTodosAssincrono();
            return Ok(lista);
        }

        /// <summary>
        /// Retorna uma entidade pelo ID.
        /// </summary>
        [HttpGet("{id:long}")]
        public virtual async Task<IActionResult> GetById(long id)
        {
            var entidade = await _servicoConsulta.ObterPorIdAssincrono(id);
            if (entidade == null)
                return NotFound();

            return Ok(entidade);
        }
    }
}
