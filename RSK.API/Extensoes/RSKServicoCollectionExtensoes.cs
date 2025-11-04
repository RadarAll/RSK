namespace RSK.API.Extensoes
{
    /// <summary>
    /// Classe de extensão responsável por registrar automaticamente
    /// todos os serviços do projeto RSK utilizando reflexão.
    ///
    /// Essa extensão permite registrar todos os serviços de uma só vez,
    /// eliminando a necessidade de adicionar cada um manualmente.
    ///
    /// Exemplo de uso:
    ///     builder.Services.AdicionarServicosRSK();
    /// </summary>
    public static class RSKServicoCollectionExtensoes
    {
        /// <summary>
        /// Registra automaticamente todos os serviços encontrados no assembly RSK.
        /// 
        /// - Registra Interface → Implementação, quando existir;
        /// - Caso não exista interface, mas o nome da classe indique que é um serviço
        ///   (ex.: termina com “Servico”, “Repositorio”, etc.), registra o tipo concreto;
        /// - Detecta atributos que indiquem o tempo de vida (Scoped, Singleton, Transient);
        /// - Se não encontrar nenhum, usa <b>Scoped</b> como padrão.
        /// </summary>
        /// <param name="servicos">Coleção de serviços (IServiceCollection)</param>
        /// <param name="tempoDeVidaPadrao">Tempo de vida padrão (Scoped por padrão)</param>
        /// <param name="prefixoNamespace">Prefixo de namespace para filtrar classes (padrão: "RSK")</param>
        /// <returns>IServiceCollection atualizada</returns>
        public static IServiceCollection AdicionarServicosRSK(this IServiceCollection servicos,
            ServiceLifetime tempoDeVidaPadrao = ServiceLifetime.Scoped,
            string prefixoNamespace = "RSK")
        {
            var assembly = typeof(RSKServicoCollectionExtensoes).Assembly;

            var tipos = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic &&
                            t.Namespace != null && t.Namespace.StartsWith(prefixoNamespace))
                .ToArray();

            foreach (var tipoImpl in tipos)
            {
                // Ignora controladores e middlewares sem construtor padrão
                if (tipoImpl.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) ||
                    (tipoImpl.Name.EndsWith("Middleware", StringComparison.OrdinalIgnoreCase) &&
                     tipoImpl.GetConstructor(Type.EmptyTypes) == null))
                {
                    continue;
                }

                var tempoDeVida = DetectarTempoDeVidaPorAtributo(tipoImpl) ?? tempoDeVidaPadrao;

                var interfaces = tipoImpl.GetInterfaces()
                    .Where(i => i.Namespace != null &&
                                !i.Namespace.StartsWith("System") &&
                                !i.Namespace.StartsWith("Microsoft"))
                    .ToArray();

                if (interfaces.Length > 0)
                {
                    foreach (var interfaceType in interfaces)
                    {
                        Registrar(servicos, interfaceType, tipoImpl, tempoDeVida);
                    }
                }
                else
                {
                    if (PareceServicoOuRepositorio(tipoImpl.Name))
                    {
                        Registrar(servicos, tipoImpl, tipoImpl, tempoDeVida);
                    }
                }
            }

            return servicos;
        }

        /// <summary>
        /// Verifica se o nome da classe sugere que é um tipo de serviço ou repositório.
        /// </summary>
        private static bool PareceServicoOuRepositorio(string nome)
        {
            string[] padroes = { "Servico", "Service", "Repositorio", "Repository", "Provider", "Manager", "Handler", "Store" };
            return padroes.Any(p => nome.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Registra um tipo e sua implementação no container de injeção de dependência.
        /// </summary>
        private static void Registrar(IServiceCollection servicos, Type tipoServico, Type tipoImpl, ServiceLifetime tempoDeVida)
        {
            bool jaRegistrado = servicos.Any(sd =>
                sd.ServiceType == tipoServico && sd.ImplementationType == tipoImpl);

            if (jaRegistrado)
                return;

            switch (tempoDeVida)
            {
                case ServiceLifetime.Singleton:
                    servicos.AddSingleton(tipoServico, tipoImpl);
                    break;
                case ServiceLifetime.Scoped:
                    servicos.AddScoped(tipoServico, tipoImpl);
                    break;
                case ServiceLifetime.Transient:
                    servicos.AddTransient(tipoServico, tipoImpl);
                    break;
                default:
                    servicos.AddScoped(tipoServico, tipoImpl);
                    break;
            }
        }

        /// <summary>
        /// Detecta o tempo de vida com base nos atributos da classe.
        /// Exemplo: [Scoped], [Transient], [Singleton].
        /// </summary>
        private static ServiceLifetime? DetectarTempoDeVidaPorAtributo(Type tipoImpl)
        {
            var atributos = tipoImpl.GetCustomAttributes(inherit: false)
                                    .Select(a => a.GetType().Name)
                                    .ToArray();

            if (atributos.Any(n => n.Contains("Singleton", StringComparison.OrdinalIgnoreCase)))
                return ServiceLifetime.Singleton;

            if (atributos.Any(n => n.Contains("Transient", StringComparison.OrdinalIgnoreCase)))
                return ServiceLifetime.Transient;

            if (atributos.Any(n => n.Contains("Scoped", StringComparison.OrdinalIgnoreCase)))
                return ServiceLifetime.Scoped;

            return null;
        }
    }

}
