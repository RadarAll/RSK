using Microsoft.AspNetCore.Mvc;
using RSK.Dominio.Autorizacao.Filtros;

namespace RSK.Dominio.Autorizacao.Atributos
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AutorizarUsuario : TypeFilterAttribute
    {
        public AutorizarUsuario() : base(typeof(FiltroUsuarioAutenticado))
        {
        }
    }
}
