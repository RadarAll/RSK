using RSK.Dominio.Entidades;

namespace RSK.Dominio.Autorizacao.Entidades
{
    public class PerfilPermissao : EntidadeBase
    {
        public long PerfilId { get; set; }
        public long PermissaoId { get; set; }
        public bool Permitido { get; set; }
        public Perfil Perfil { get; set; } = default!;
        public Permissao Permissao { get; set; } = default!;

        private PerfilPermissao() { }
    }
}
