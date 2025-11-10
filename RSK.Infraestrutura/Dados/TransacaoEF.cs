using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using RSK.Dominio.IRepositorios;
using RSK.Dominio.Notificacoes.Interfaces;

namespace RSK.Infraestrutura.Dados
{
    public class TransacaoEF<TContext> : ITransacao where TContext : DbContext
    {
        private readonly INotificador _notificador;
        private readonly TContext _contexto;
        private IDbContextTransaction? _transacaoAtual;

        public TransacaoEF(TContext contexto, INotificador notificador)
        {
            _contexto = contexto;
            _notificador = notificador;

            // Inicia a transação imediatamente ao ser instanciado no escopo (Scoped)
            // Isso garante que todas as operações dentro deste escopo usem a mesma transação.
            _transacaoAtual = _contexto.Database.BeginTransaction();
        }

        /// <summary>
        /// Confirma todas as alterações e a transação de banco de dados.
        /// </summary>
        public async Task<int> CommitAssincrono()
        {
            if (_transacaoAtual == null)
            {
                throw new InvalidOperationException("A transação não foi iniciada.");
            }

            try
            {
                var resultado = await _contexto.SaveChangesAsync();
                await _transacaoAtual.CommitAsync();

                return resultado;
            }
            catch (Exception)
            {
                await RollbackAssincrono();
                throw;
            }
            finally
            {
                _transacaoAtual.Dispose();
                _transacaoAtual = null;
            }
        }

        /// <summary>
        /// Desfaz todas as alterações pendentes.
        /// </summary>
        public async Task RollbackAssincrono()
        {
            try
            {
                if (_transacaoAtual != null)
                {
                    await _transacaoAtual.RollbackAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro durante o Rollback: {ex.Message}");
            }
        }

        /// <summary>
        /// Garante que o rollback ocorra se a classe for descartada sem um commit.
        /// </summary>
        public void Dispose()
        {
            if (_transacaoAtual != null)
            {
                try
                {
                    _transacaoAtual.Rollback();
                }
                catch
                {
                    // ignora, já foi commit/rollback
                }
                finally
                {
                    _transacaoAtual.Dispose();
                    _transacaoAtual = null;
                }
            }
        }

    }
}
