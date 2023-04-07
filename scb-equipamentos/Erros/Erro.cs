namespace scb_equipamentos.Erros
{
    public class Erro
    {
        public string Codigo { get; set; }

        public string Mensagem { get; set; }

        public Erro(string codigo, string mensagem)
        {
            Codigo = codigo;
            Mensagem = mensagem;
        }
    }
}
