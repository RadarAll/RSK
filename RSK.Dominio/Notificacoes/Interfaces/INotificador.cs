using RSK.Dominio.Notificacoes.Entidades;

namespace RSK.Dominio.Notificacoes.Interfaces
{
    public interface INotificador
    {
        bool PossuiMensagens();
        bool PossuiErros();
        IReadOnlyCollection<Mensagem> ObterMensagens();
        IEnumerable<Mensagem> ObterErros();
        string ToString();
    }
}
