namespace RSK.Dominio.IRepositorios
{
    public interface ITransacao : IDisposable
    {
        Task<int> CommitAssincrono();
        Task RollbackAssincrono();
    }
}
