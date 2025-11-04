namespace RSK.Dominio.Autorizacao.DTOs
{
    public class UsuarioBaseDTOs
    {
        public class UsuarioBaseDTOInput
        {
            public string NomeCompleto { get; set; } = default!;
            public string Email { get; set; } = default!;
            public string Senha { get; set; } = default!;
            public DateTime? DataDesativacao { get; set; }
        }
        
        public class UsuarioBaseDTOOutput : UsuarioBaseDTOInput
        {
            public bool Ativo { get; set; }
        }
    }
}
