
using RSK.Dominio.Autorizacao.Interfaces;
using RSK.Dominio.Autorizacao.Servicos;

namespace RSK.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IServicoVerificacaoAplicacao, ServicoVerificacaoAplicacao>();
            builder.Services.AddScoped<RSK.Dominio.Autorizacao.Filtros.FiltroAutorizarGenerico>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
