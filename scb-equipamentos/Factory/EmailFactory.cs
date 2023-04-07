using scb_equipamentos.Models;

namespace scb_equipamentos.Factory
{
    public class EmailFactory
    {
        public static NovoEmail? Create(string email, string assunto, string mensagem)
        {
            return new NovoEmail(email, assunto, mensagem);

        }
    }
}
