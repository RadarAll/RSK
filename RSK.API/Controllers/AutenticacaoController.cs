using Microsoft.AspNetCore.Mvc;
using RSK.Dominio.Autorizacao.Interfaces;
using RSK.Dominio.Notificacoes.Interfaces;

namespace RSK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IServicoAutenticacao _servicoAutenticacao;
        private readonly INotificador _notificador;

        public AutenticacaoController(
            IServicoAutenticacao servicoAutenticacao,
            INotificador notificador
        )
        {
            _servicoAutenticacao = servicoAutenticacao;
            _notificador = notificador;
        }

        /// <summary>
        /// Endpoint para login de usuário.
        /// Retorna token JWT em caso de sucesso.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
            {
                return BadRequest(new
                {
                    mensagens = new[] { new { Texto = "E-mail e senha são obrigatórios.", Tipo = "Erro" } }
                });
            }

            var token = await _servicoAutenticacao.AutenticarAsync(request.Email, request.Senha);

            if (_notificador.PossuiErros())
            {
                return Unauthorized(new
                {
                    mensagens = _notificador.ObterMensagens()
                });
            }

            return Ok(new
            {
                mensagens = new[] { new { Texto = "Login realizado com sucesso.", Tipo = "Sucesso" } },
                token
            });
        }
    }

    /// <summary>
    /// DTO para requisição de login
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; } = default!;
        public string Senha { get; set; } = default!;
    }
}
