using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RSK.Dominio.Autorizacao.Entidades;
using RSK.Dominio.Autorizacao.Interfaces;
using RSK.Dominio.Notificacoes.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RSK.Dominio.Autorizacao.Servicos
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly IServicoUsuario _servicoUsuario;
        private readonly IServicoMensagem _mensagens;
        private readonly IConfiguration _config;

        public ServicoAutenticacao(
            IServicoUsuario servicoUsuario,
            IServicoMensagem mensagens,
            IConfiguration config)
        {
            _servicoUsuario = servicoUsuario;
            _mensagens = mensagens;
            _config = config;
        }

        public async Task<string?> AutenticarAsync(string email, string senha)
        {
            var usuario = await _servicoUsuario.ObterPorEmail(email);
            if (usuario == null)
            {
                _mensagens.AdicionarErro("Usuário não encontrado.");
                return null;
            }

            if (!await _servicoUsuario.VerificarSenhaAsync(usuario, senha))
            {
                _mensagens.AdicionarErro("Senha incorreta.");
                return null;
            }

            if (!usuario.Ativo)
            {
                _mensagens.AdicionarErro("Usuário desativado.");
                return null;
            }

            return GerarToken(usuario);
        }

        private string GerarToken(UsuarioBase usuario)
        {
            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("nome", usuario.NomeCompleto),
                new Claim("usuarioId", usuario.Id.ToString())
            };

            var keyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var creds = new SigningCredentials(keyBytes, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
