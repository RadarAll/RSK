using RSK.Dominio.Entidades;

namespace RSK.Dominio.Autorizacao.Entidades
{
    public class AplicacaoPermitida : EntidadeBase
    {
        public string Nome { get; set; }
        public string IntegrationSecretHash { get; set; }
        public bool Ativa { get; set; }

        private AplicacaoPermitida() {}
    }
}
