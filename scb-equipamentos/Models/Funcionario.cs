using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

namespace scb_equipamentos.Models
{
    public class Funcionario
    {
        [JsonPropertyName("matricula")]
        public string Matricula { get; set; }

        [JsonPropertyName("senha")]
        public string Senha { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("idade")]
        public int Idade { get; set; }


        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }

        public Funcionario(string matricula, string senha, string email, string nome, int idade, string cpf)
        {
            Matricula = matricula;
            Senha = senha;
            Email = email;
            Nome = nome;
            Idade = idade;
            Cpf = cpf;
        }
    }

}
