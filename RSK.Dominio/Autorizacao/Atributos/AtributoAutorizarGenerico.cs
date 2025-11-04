using Microsoft.AspNetCore.Mvc;

namespace RSK.Dominio.Autorizacao.Atributos
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AutorizarGenerico : TypeFilterAttribute
    {
        public AutorizarGenerico() : base(typeof(Filtros.FiltroAutorizarGenerico))
        {
        }
    }
}

