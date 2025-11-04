using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSK.Dominio.Autorizacao.DTOs;
using RSK.Dominio.Autorizacao.Entidades;
using RSK.Dominio.Autorizacao.Interfaces;
using RSK.Dominio.Interfaces;
using RSK.Dominio.Notificacoes.Interfaces;

namespace RSK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ApiCrudControllerBase<UsuarioBase>
    {
        private readonly IServicoUsuario _servicoUsuario;

        public UsuarioController(
            IServicoConsultaBase<UsuarioBase> servicoConsulta,
            IServicoUsuario servicoUsuario,
            INotificador notificador
        ) : base(servicoConsulta, servicoUsuario, notificador)
        {
            _servicoUsuario = servicoUsuario;
        }

        /// <summary>
        /// Endpoint específico para buscar usuário por e-mail.
        /// </summary>
        [HttpGet("email/{email}")]
        public async Task<IActionResult> ObterPorEmail(string email)
        {
            var usuario = await _servicoUsuario.ObterPorEmail(email);
            if (usuario == null)
                return NotFound(new
                {
                    mensagens = new[] { new { Texto = "Usuário não encontrado.", Tipo = "Erro" } }
                });

            var dto = new UsuarioBaseDTOs.UsuarioBaseDTOOutput
            {
                NomeCompleto = usuario.NomeCompleto,
                Email = usuario.Email,
                Ativo = usuario.Ativo,
                DataDesativacao = usuario.DataDesativacao
            };

            return Ok(new
            {
                mensagens = new[] { new { Texto = "Usuário encontrado com sucesso.", Tipo = "Sucesso" } },
                dados = dto
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] UsuarioBase entidade)
        {
            var resultado = await _servicoUsuario.AdicionarAssincrono(entidade);

            if (_servicoMensagem.PossuiErros() || resultado == null)
                return BadRequest(new { mensagens = _servicoMensagem.ObterMensagens() });

            return Created(string.Empty, new
            {
                mensagens = new[] { new { Texto = $"{typeof(UsuarioBase).Name} criada com sucesso.", Tipo = "Sucesso" } },
                dados = resultado
            });
        }

    }
}
