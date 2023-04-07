using System.ComponentModel.DataAnnotations;

namespace scb_equipamentos.Data.BicicletaDto
{
    public class CreateBicicletaDto
    {
        [Required(ErrorMessage = "O campo marca é obrigatório")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "O campo modelo é obrigatório")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O campo ano de fabricação é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = " O ano de fabricação deve ser maior que zero")]
        public int AnoFabricacao { get; set; }

    }
}
