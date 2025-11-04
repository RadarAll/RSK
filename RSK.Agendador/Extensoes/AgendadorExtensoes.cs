using Microsoft.Extensions.DependencyInjection;
using Quartz;
using RSK.Agendador.Enums;
using RSK.Agendador.Jobs;
using RSK.Agendador.Util;

namespace RSK.Agendador.Extensoes
{
    public static class AgendadorExtensoes
    {
        /// <summary>
        /// Registra um job genérico para o serviço e método especificado.
        /// Agora permite definir um horário de início com TimeOnly.
        /// </summary>
        public static IServiceCollection AdicionarJobServico<TServico>(
            this IServiceCollection services,
            string nomeJob,
            string metodo,
            TipoAgendamento tipoAgendamento,
            int intervalo = 1,
            DiaSemana diaSemana = DiaSemana.Segunda,
            TimeOnly? horaInicio = null)
            where TServico : class
        {
            // Define o cron com base no tipo de agendamento
            string cron = tipoAgendamento switch
            {
                TipoAgendamento.Segundos => CronIntervalo.ACadaSegundos(intervalo),
                TipoAgendamento.Minutos => CronIntervalo.ACadaMinutos(intervalo),
                TipoAgendamento.Horas => CronIntervalo.ACadaHoras(intervalo),
                TipoAgendamento.Diario =>
                    horaInicio.HasValue
                        ? $"0 {horaInicio.Value.Minute} {horaInicio.Value.Hour} * * ?"
                        : CronIntervalo.Diariamente(2, 0), // default 02:00
                TipoAgendamento.Semanal =>
                    horaInicio.HasValue
                        ? $"0 {horaInicio.Value.Minute} {horaInicio.Value.Hour} ? * {(int)diaSemana + 1}"
                        : CronIntervalo.Semanalmente(diaSemana, 2, 0),
                TipoAgendamento.Mensal =>
                    horaInicio.HasValue
                        ? $"0 {horaInicio.Value.Minute} {horaInicio.Value.Hour} 1 * ?"
                        : CronIntervalo.Mensalmente(1, 2, 0),
                TipoAgendamento.MeiaNoite => CronIntervalo.MeiaNoite(),
                _ => throw new ArgumentOutOfRangeException(nameof(tipoAgendamento))
            };

            services.AddQuartz(q =>
            {
                var jobKey = new JobKey(nomeJob, "Servicos");

                q.AddJob<JobServicoGenerico<TServico>>(opts => opts
                    .WithIdentity(jobKey)
                    .UsingJobData("Metodo", metodo));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity($"{nomeJob}-trigger", "Servicos")
                    .WithCronSchedule(cron));
            });

            // Adiciona o Quartz Hosted Service
            services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

            return services;
        }
    }
}
