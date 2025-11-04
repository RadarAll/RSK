using RSK.Agendador.Enums;

namespace RSK.Agendador.Configuracoes
{
    public class AgendamentoServico
    {
        public Type TipoServico { get; set; } = default!;
        public TipoAgendamento TipoAgendamento { get; set; }
        public int Intervalo { get; set; } = 1;
        public DayOfWeek? DiaSemana { get; set; }
        public int Hora { get; set; } = 0;
        public int Minuto { get; set; } = 0;
        public string? Descricao { get; set; }
    }
}
