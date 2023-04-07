namespace scb_equipamentos.Data.TrancaDto
{
    public class ReturnTrancaDto
    {
        public int Id { get; set; }
        public int? BicicletaId { get; set; }
        public int Numero { get; set; }
        public string Localizacao { get; set; }
        public int AnoFabricacao { get; set; }
        public string Modelo { get; set; }
        public string Status { get; set; }
    }
}
