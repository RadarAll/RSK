namespace RSK.Agendador.Interfaces
{
    public interface IServicoAgendavel
    {
        Task<bool> ExecutarAsync();
        string NomeServico { get; }
    }
}
