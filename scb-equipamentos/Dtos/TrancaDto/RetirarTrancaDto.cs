using System.ComponentModel.DataAnnotations;

namespace scb_equipamentos.Data.TrancaDto
{
    public class RetirarTrancaDto
    {
        [Required(ErrorMessage = "O campo idTotem é obrigatório")]
        public int IdTotem { get; set; }

        [Required(ErrorMessage = "O campo idTranca é obrigatório")]
        public int IdTranca { get; set; }

        [Required(ErrorMessage = "O campo idFuncionario é obrigatório")]
        public int IdFuncionario { get; set; }

        [Required(ErrorMessage = "O campo statusAcaoReparador é obrigatório")]
        public string StatusAcaoReparador { get; set; }
    }
}
