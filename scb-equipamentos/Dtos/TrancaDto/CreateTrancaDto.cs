using System.ComponentModel.DataAnnotations;

namespace scb_equipamentos.Data.TrancaDto
{
    public class CreateTrancaDto
    {
        [Required(ErrorMessage = "O campo número é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Número deve ser maior que 1")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "O campo localização é obrigatório")]
        public string Localizacao { get; set; }

        [Required(ErrorMessage = "O campo ano de fabricação é obrigatório")]
        [Range(1, 9999, ErrorMessage = "Ano de fabricação deve estar entre 1 e 9999")]
        public int AnoFabricacao { get; set; }

        [Required(ErrorMessage = "O campo modelo é obrigatório")]
        public string Modelo { get; set; }
    }
}
