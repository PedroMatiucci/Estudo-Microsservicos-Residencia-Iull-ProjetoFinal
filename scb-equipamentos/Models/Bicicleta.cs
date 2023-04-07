using scb_equipamentos.Extensions;
using scb_equipamentos.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace scb_equipamentos.Models
{
    public class Bicicleta
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo número é obrigatório")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "O campo marca é obrigatório")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "O campo modelo é obrigatório")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O campo ano de fabricação é obrigatório")]
        [Range (0, int.MaxValue, ErrorMessage = " O ano de fabricação deve ser maior que zero")]
        public int AnoFabricacao { get; set; }

        public Tranca? Tranca { get; set; }

        internal StatusBicicleta StatusEnum
        {
            get { return Status.ParaValorBicicleta(); }
            set { Status = value.ParaStringBicicleta(); }
        }
        public string Status { get; private set; }

    }
}
