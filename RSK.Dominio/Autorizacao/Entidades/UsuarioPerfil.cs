using RSK.Dominio.Entidades;

namespace RSK.Dominio.Autorizacao.Entidades
{
    public class UsuarioPerfil : EntidadeBase
    {
        public long UsuarioId { get; set; }
        public long PerfilId { get; set; }
        public Perfil Perfil { get; set; } = default!;

        private UsuarioPerfil() { }
    }
}
