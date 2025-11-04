using RSK.Dominio.Notificacoes.Entidades;
using RSK.Dominio.Notificacoes.Interfaces;
using System.Text;

namespace RSK.Dominio.Notificacoes.Servicos
{
    public class ServicoMensagem : IServicoMensagem, INotificador, IDisposable
    {
        private readonly List<Mensagem> _mensagens = new();


        public void Adicionar(string texto, TipoMensagem tipo = TipoMensagem.Informacao)
        {
            if (!string.IsNullOrWhiteSpace(texto))
            {
                _mensagens.Add(new Mensagem(texto, tipo));
            }
        }
        public void AdicionarErro(string texto)
        {
            Adicionar(texto, TipoMensagem.Erro);
        }
        public void AdicionarAviso(string texto)
        {
            Adicionar(texto, TipoMensagem.Aviso);
        }




        public bool PossuiMensagens()
        {
            return _mensagens.Any();
        }
        public bool PossuiErros()
        {
            return _mensagens.Any(m => m.Tipo == TipoMensagem.Erro);
        }
        public IReadOnlyCollection<Mensagem> ObterMensagens()
        {
            return _mensagens.AsReadOnly();
        }
        public IEnumerable<Mensagem> ObterErros()
        {
            return _mensagens.Where(m => m.Tipo == TipoMensagem.Erro);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (!_mensagens.Any()) return "Nenhuma mensagem registrada.";
            foreach (var msg in _mensagens) builder.AppendLine($"[{msg.Tipo}] {msg.Texto}");
            return builder.ToString().TrimEnd();
        }



        public void Limpar()
        {
            _mensagens.Clear();
        }

        public void Dispose()
        {
            Limpar();
        }
    }
}