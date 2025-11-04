using RSK.Dominio.Entidades;

namespace RSK.Dominio.Autorizacao.Entidades
{
    public class UsuarioBase : EntidadeBase
    {
        public string NomeCompleto { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string SenhaHash { get; set; } = default!;
        public bool Ativo { get; set; } = true;
        public DateTime? DataDesativacao { get; set; } = null;

        public UsuarioBase() { }
        //private UsuarioBase() { }
    }
}
