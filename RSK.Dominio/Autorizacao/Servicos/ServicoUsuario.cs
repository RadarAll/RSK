using RSK.Dominio.Autorizacao.Entidades;
using RSK.Dominio.Autorizacao.Interfaces;
using RSK.Dominio.IRepositorios;
using RSK.Dominio.Notificacoes.Interfaces;
using RSK.Dominio.Servicos;
using System.Text.RegularExpressions;


namespace RSK.Dominio.Autorizacao.Servicos
{
    public class ServicoUsuario : ServicoCrudBase<UsuarioBase>, IServicoUsuario
    {
        private readonly IHasher _hasher;

        public ServicoUsuario(
            IRepositorioBaseAssincrono<UsuarioBase> repositorio,
            IServicoMensagem mensagens,
            ITransacao transacao,
            IHasher hasher
        ) : base(repositorio, mensagens, transacao)
        {
            _hasher = hasher;
        }

        /// <summary>
        /// Sobrescreve a validação base para aplicar regras específicas de usuários.
        /// </summary>
        protected override bool Validar(UsuarioBase? usuario, string operacao = "")
        {
            if (usuario == null)
            {
                _mensagens.AdicionarErro($"Usuário é nulo na operação {operacao}.");
                return false;
            }

            bool valido = true;

            // Nome obrigatório
            if (string.IsNullOrWhiteSpace(usuario.NomeCompleto))
            {
                _mensagens.AdicionarErro("O nome completo é obrigatório.");
                valido = false;
            }

            // E-mail obrigatório e válido
            if (string.IsNullOrWhiteSpace(usuario.Email))
            {
                _mensagens.AdicionarErro("O e-mail é obrigatório.");
                valido = false;
            }
            else if (!Regex.IsMatch(usuario.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                _mensagens.AdicionarErro("O e-mail informado é inválido.");
                valido = false;
            }

            // Senha obrigatória e com tamanho mínimo (apenas em criação)
            if (operacao.Equals("Adicionar", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(usuario.SenhaHash))
                {
                    _mensagens.AdicionarErro("A senha é obrigatória.");
                    valido = false;
                }
                else if (usuario.SenhaHash.Length < 8)
                {
                    _mensagens.AdicionarErro("A senha deve conter no mínimo 8 caracteres.");
                    valido = false;
                }
            }

            return valido;
        }

        public override async Task<UsuarioBase?> AdicionarAssincrono(UsuarioBase usuario)
        {
            if (!Validar(usuario, "Adicionar"))
                return null;

            // Impede duplicação por e-mail
            var existente = await _repositorio.BuscarAssincrono(u => u.Email == usuario.Email);
            if (existente.Any())
            {
                _mensagens.AdicionarErro("Já existe um usuário com este e-mail.");
                return null;
            }

            // Hash da senha
            usuario.SenhaHash = _hasher.Hash(usuario.SenhaHash);

            // Define ativo com base na DataDesativacao
            usuario.Ativo = usuario.DataDesativacao == default || usuario.DataDesativacao > DateTime.UtcNow;

            await _repositorio.AdicionarAssincrono(usuario);
            await _transacao.CommitAssincrono();

            return usuario;
        }

        public override async Task<UsuarioBase?> AtualizarAssincrono(UsuarioBase usuario)
        {
            if (!Validar(usuario, "Atualizar"))
                return null;

            // Atualiza o status ativo caso a data de desativação tenha sido alterada
            usuario.Ativo = usuario.DataDesativacao == default || usuario.DataDesativacao > DateTime.UtcNow;

            usuario.AtualizarDataAlteracao();
            _repositorio.AtualizarAssincrono(usuario);
            await _transacao.CommitAssincrono();

            return usuario;
        }


        public async Task<UsuarioBase?> ObterPorEmail(string email)
        {
            var usuarios = await _repositorio.BuscarAssincrono(u => u.Email == email);
            return usuarios.FirstOrDefault();
        }

        public async Task<bool> VerificarSenhaAsync(UsuarioBase usuario, string senhaDigitada)
        {
            var senhaHash = _hasher.Hash(senhaDigitada);
            return usuario.SenhaHash == senhaHash;
        }
    }
}

