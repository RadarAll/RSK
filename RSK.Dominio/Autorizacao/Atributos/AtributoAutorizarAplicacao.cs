using Microsoft.AspNetCore.Mvc;
using RSK.Dominio.Autorizacao.Filtros;

namespace RSK.Dominio.Autorizacao.Atributos
{
    public class AutorizarAplicacao : TypeFilterAttribute
    {
        public AutorizarAplicacao() : base(typeof(FiltroAplicacaoPermitidaPermissao))
        {
        }
    }
}
