using scb_equipamentos.Extensions;
using scb_equipamentos.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace scb_equipamentos.Models
{
    public class Tranca
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [JsonIgnore]
        public Bicicleta? Bicicleta { get; set; }

        [Required(ErrorMessage = "O campo número é obrigatório")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "O campo localização é obrigatório")]
        public string Localizacao { get; set; }

        [Required(ErrorMessage = "O campo ano de fabricação é obrigatório")]
        public int AnoFabricacao { get; set; }

        [Required(ErrorMessage = "O campo modelo é obrigatório")]
        public string Modelo { get; set; }

        internal StatusTranca StatusEnum
        {
            get { return Status.ParaValorTranca(); }
            set { Status = value.ParaStringTranca(); }
        }
        public string Status { get; private set; }

        public int? BicicletaId { get; set; }

        public bool isExcluida()
        {
            return StatusEnum == StatusTranca.EXCLUIDA;
        }
    }
}
