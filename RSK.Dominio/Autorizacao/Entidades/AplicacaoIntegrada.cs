using RSK.Dominio.Entidades;

namespace RSK.Dominio.Autorizacao.Entidades
{
    public class AplicacaoIntegrada : EntidadeBase
    {
        public string Nome { get; set; }
        public string IntegrationSecretHash { get; set; }
        public string Url { get; set; }
        public bool Ativa { get; set; }

        private AplicacaoIntegrada() { }
    }
}
