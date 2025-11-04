using RSK.Dominio.Entidades;

namespace RSK.Dominio.Autorizacao.Entidades
{
    public class Permissao : EntidadeBase
    {
        public string Controller { get; }
        public string Action { get; }

        public Permissao(string controller, string action)
        {
            Controller = controller;
            Action = action;
        }

        public override string ToString() => $"{Controller}.{Action}";


        private Permissao() { }
    }
}
