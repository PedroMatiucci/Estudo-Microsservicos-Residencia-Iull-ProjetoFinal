using System.ComponentModel.DataAnnotations;

namespace scb_equipamentos.Models
{
    public class Totem
    {
        [Key]
        [Required(ErrorMessage = "O campo número é obrigatório")]
        public int Numero { get; set; }
        [Required(ErrorMessage = "O campo localização é obrigatório")]
        public string Localizacao { get; set; }
        public List<Tranca> Trancas { get; set; }
    }
}
