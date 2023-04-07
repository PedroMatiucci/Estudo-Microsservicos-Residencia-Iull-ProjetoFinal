using scb_equipamentos.Models;
using System.Text.Json;

namespace scb_equipamentos.Services
{
    public class AluguelAPI
    {

        public static Funcionario? RetornaFuncionarioPorId(int idFuncionario) { 
            var url = $"https://residencia-nebula.ed.dev.br/aluguel-grupo1/funcionario/{idFuncionario}";
            var client = new HttpClient();
            try {
                var json = client.GetStringAsync(url).Result;
                return JsonSerializer.Deserialize<Funcionario>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}
