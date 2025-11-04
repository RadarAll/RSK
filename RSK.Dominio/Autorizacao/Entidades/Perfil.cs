using RSK.Dominio.Entidades;

namespace RSK.Dominio.Autorizacao.Entidades
{
    public class Perfil : EntidadeBase
    {
        public string Nome { get; set; } = default!;

        private Perfil() { }
    }
}
