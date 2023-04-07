using System.ComponentModel.DataAnnotations;

namespace scb_equipamentos.Data.TotemDto
{
    public class UpdateTotemDto
    {
        [Required(ErrorMessage = "O campo localização é obrigatório")]
        [RegularExpression(@"^[a-zA-ZãõáéíóúâêôÁÉÍÓÚÂÊÔ\s]{1,40}$")]
        public string Localizacao { get; set; }
    }
}
