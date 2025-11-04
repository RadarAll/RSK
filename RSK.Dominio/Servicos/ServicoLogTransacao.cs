using Microsoft.Extensions.Logging;
using RSK.Dominio.Interfaces;
using RSK.Dominio.Notificacoes.Interfaces;


namespace RSK.Dominio.Servicos
{
    public class ServicoLogTransacao : IServicoLogTransacao
    {
        private readonly ILogger<ServicoLogTransacao> _logger;
        private readonly INotificador _notificador;


        public ServicoLogTransacao(ILogger<ServicoLogTransacao> logger, INotificador notificador)
        {
            _logger = logger;
            _notificador = notificador;
        }

        public void LogarResultadoTransacao(string operacao)
        {
            var mensagensDetalhes = _notificador.ToString();

            try
            {
                if (_notificador.PossuiErros())
                {
                    _logger.LogError("Falha na operação [{Operacao}]. Detalhes:\n{Mensagens}", operacao, mensagensDetalhes);
                }
                else if (_notificador.PossuiMensagens())
                {
                    _logger.LogInformation("Sucesso na operação [{Operacao}]. Mensagens:\n{Mensagens}", operacao, mensagensDetalhes);
                }
                else
                {
                    _logger.LogDebug("Operação [{Operacao}] concluída sem notificações a registrar.", operacao);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Falha crítica ao tentar logar o resultado da operação [{Operacao}].", operacao);
            }
        }
    }
}