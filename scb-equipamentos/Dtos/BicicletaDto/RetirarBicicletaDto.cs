namespace scb_equipamentos.Data.BicicletaDto
{
    public class RetirarBicicletaDto
    {
        public int IdBicicleta { get; set; }
        
        public int IdTranca { get; set; }

        public int IdFuncionario { get; set; }

        public string StatusAcaoReparador { get; set; }
    }
}
