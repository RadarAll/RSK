using System.ComponentModel.DataAnnotations;

namespace RSK.Dominio.Entidades
{
    public abstract class EntidadeBase
    {
        [Key]
        public long Id { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime UltimaAlteracao { get; set; }

        protected EntidadeBase()
        {
            CriadoEm = DateTime.UtcNow;
            UltimaAlteracao = DateTime.UtcNow;
        }

        public void AtualizarDataAlteracao()
        {
            UltimaAlteracao = DateTime.UtcNow;
        }
    }
}
