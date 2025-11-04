using RSK.Dominio.Entidades;

namespace RSK.Dominio.Autorizacao.Entidades
{
    public class AplicacaoPermitidaPermissao : EntidadeBase
    {
        public long AplicacaoPermitidaId { get; set; }
        public string Controller { get; set; }
        public string Acao { get; set; }
        public AplicacaoPermitida AplicacaoPermitida { get; set; }

        private AplicacaoPermitidaPermissao() { }
    }
}
