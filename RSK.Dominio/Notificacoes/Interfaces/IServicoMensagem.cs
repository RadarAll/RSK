using RSK.Dominio.Notificacoes.Entidades;

namespace RSK.Dominio.Notificacoes.Interfaces
{
    public interface IServicoMensagem
    {
        void Adicionar(string texto, TipoMensagem tipo = TipoMensagem.Informacao);
        void AdicionarErro(string texto);
        void AdicionarAviso(string texto);
    }
}
