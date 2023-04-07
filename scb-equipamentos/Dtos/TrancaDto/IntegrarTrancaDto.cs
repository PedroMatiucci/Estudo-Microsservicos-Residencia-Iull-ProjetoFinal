using System.ComponentModel.DataAnnotations;

namespace scb_equipamentos.Data.TrancaDto
{
    public class IntegrarTrancaDto
    {
        [Required(ErrorMessage = "O campo idTotem é obrigatório")]
        public int IdTotem { get; set; }

        [Required(ErrorMessage = "O campo idTranca é obrigatório")]
        public int IdTranca { get; set; }

        [Required(ErrorMessage = "O campo idFuncionario é obrigatório")]
        public int IdFuncionario { get; set; }
    }
}
