using RSK.Agendador.Enums;

namespace RSK.Agendador.Util
{
    /// <summary>
    /// Classe auxiliar responsável por gerar expressões CRON de forma legível e em português.
    /// </summary>
    public static class CronIntervalo
    {
        /// <summary>
        /// Executa a cada N segundos.
        /// </summary>
        public static string ACadaSegundos(int segundos)
            => $"0/{segundos} * * * * ?";

        /// <summary>
        /// Executa a cada N minutos.
        /// </summary>
        public static string ACadaMinutos(int minutos)
            => $"0 0/{minutos} * * * ?";

        /// <summary>
        /// Executa a cada N horas.
        /// </summary>
        public static string ACadaHoras(int horas)
            => $"0 0 0/{horas} * * ?";

        /// <summary>
        /// Executa todos os dias no horário informado.
        /// </summary>
        /// <param name="hora">Hora (0-23).</param>
        /// <param name="minuto">Minuto (0-59).</param>
        public static string Diariamente(int hora, int minuto = 0)
            => $"0 {minuto} {hora} * * ?";

        /// <summary>
        /// Executa semanalmente em um dia da semana específico e horário informado.
        /// </summary>
        public static string Semanalmente(DiaSemana dia, int hora, int minuto = 0)
        {
            var nomeDia = dia.ToString().Substring(0, 3).ToUpper();
            return $"0 {minuto} {hora} ? * {nomeDia}";
        }

        /// <summary>
        /// Executa mensalmente no dia e horário especificados.
        /// </summary>
        /// <param name="dia">Dia do mês (1-31).</param>
        /// <param name="hora">Hora (0-23).</param>
        /// <param name="minuto">Minuto (0-59).</param>
        public static string Mensalmente(int dia, int hora, int minuto = 0)
            => $"0 {minuto} {hora} {dia} * ?";

        /// <summary>
        /// Executa automaticamente todos os dias à meia-noite.
        /// </summary>
        public static string MeiaNoite()
            => "0 0 0 * * ?";
    }
}
