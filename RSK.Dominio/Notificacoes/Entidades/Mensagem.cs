namespace RSK.Dominio.Notificacoes.Entidades
{
    /// <summary>
    /// Representa uma mensagem de notificação do sistema (erro, sucesso, aviso ou informação).
    /// </summary>
    public class Mensagem
    {
        public string Texto { get; }
        public TipoMensagem Tipo { get; }

        public Mensagem(string texto, TipoMensagem tipo = TipoMensagem.Informacao)
        {
            Texto = texto?.Trim() ?? string.Empty;
            Tipo = tipo;
        }

        public override string ToString() => $"{Tipo}: {Texto}";
    }

    public enum TipoMensagem
    {
        Sucesso,
        Erro,
        Aviso,
        Informacao
    }
}
