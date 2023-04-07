using System.ComponentModel.DataAnnotations;

namespace scb_equipamentos.Models
{
    public class NovoEmail
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Assunto { get; set; }

        [Required]
        public string Mensagem { get; set; }

        public NovoEmail(string email, string assunto, string mensagem)
        {
            Email = email;
            Assunto = assunto;
            Mensagem = mensagem;
        }
    }
}
