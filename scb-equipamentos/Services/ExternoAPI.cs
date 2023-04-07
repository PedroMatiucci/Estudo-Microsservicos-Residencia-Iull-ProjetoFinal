using scb_equipamentos.Models;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace scb_equipamentos.Services
{
    public class ExternoAPI
    {
        public static async Task<HttpStatusCode> EnviarEmail(NovoEmail email)
        {
            JsonElement root;
            var url = $"https://residencia-nebula.ed.dev.br/externo-grupo1/enviarEmail";
            using (var client = new HttpClient())
            {
               var json = JsonSerializer.Serialize(email);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json),
                };

                var response = await client.SendAsync(request).ConfigureAwait(false);

                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                
                JsonDocument info = JsonDocument.Parse(responseBody);
                root = info.RootElement;
                var id = root.GetProperty("id").Deserialize<int>();
                return response.StatusCode;
            }

        }
    }
}
