using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Reflection;

namespace RSK.Agendador.Jobs
{
    /// <summary>
    /// Job genérico que executa métodos assíncronos de serviços (ex: ImportarAsync).
    /// </summary>
    public class JobServicoGenerico<TServico> : IJob where TServico : class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<JobServicoGenerico<TServico>> _logger;

        public JobServicoGenerico(IServiceProvider serviceProvider, ILogger<JobServicoGenerico<TServico>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// Executa o método definido para o serviço genérico.
        /// </summary>
        public async Task Execute(IJobExecutionContext context)
        {
            string? metodo = context.JobDetail.JobDataMap.GetString("Metodo");
            if (string.IsNullOrWhiteSpace(metodo))
            {
                _logger.LogWarning("Nenhum método informado para execução no serviço {Servico}.", typeof(TServico).Name);
                return;
            }

            try
            {
                var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
                if (scopeFactory == null)
                {
                    _logger.LogError("IServiceScopeFactory não registrado no ServiceProvider.");
                    return;
                }

                using var scope = scopeFactory.CreateScope();
                var servico = scope.ServiceProvider.GetService<TServico>();
                if (servico == null)
                {
                    _logger.LogError("Serviço {Servico} não registrado no container.", typeof(TServico).Name);
                    return;
                }

                var metodoInfo = typeof(TServico).GetMethod(metodo, BindingFlags.Public | BindingFlags.Instance);
                if (metodoInfo == null)
                {
                    _logger.LogError("Método '{Metodo}' não encontrado no serviço {Servico}.", metodo, typeof(TServico).Name);
                    return;
                }

                // Verifica se o método é assíncrono (retorna Task)
                var resultado = metodoInfo.Invoke(servico, null);
                if (resultado is Task tarefa)
                    await tarefa;

                _logger.LogInformation("Serviço {Servico} executado com sucesso pelo método {Metodo}.", typeof(TServico).Name, metodo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar o serviço {Servico} no método {Metodo}.", typeof(TServico).Name, metodo);
            }
        }
    }
}
