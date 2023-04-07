namespace TesteEquipamento.Dados
{
    public class TrancaDados
    {
        public static IEnumerable<object[]> IncluirTrancasInvalidasDados => new List<object[]> { 
            new object[] { 0, "Rio de Janeiro", 2020, "Teste" },
            new object[] { 1, null, 2020, "Teste" },
            new object[] { 1, "Rio de Janeiro", 0, "Teste" },
            new object[] { 1, "Rio de Janeiro", 2020, null },
            new object[] { 21, "Rio de Janeiro", 2020, "Teste" },
            new object[] { 1, "Rio de Janeiro", 10000, "Teste" },
            new object[] { 1, "Rio de Janeiro", -1, "Teste" },
            new object[] { 1, "", 2020, "Teste" },
            new object[] { 1, "Rio de Janeiro", 2020, "" }
        };

        public static IEnumerable<object[]> EditarTrancasInvalidasDados => new List<object[]> {
            new object[] { 0, "Rio de Janeiro", 2020, "Teste", "NOVA" },
            new object[] { 1, null, 2020, "Teste", "NOVA" },
            new object[] { 1, "Rio de Janeiro", 0, "Teste", "NOVA" },
            new object[] { 1, "Rio de Janeiro", 2020, null, "NOVA" },
            new object[] { 1, "Rio de Janeiro", 0, "Teste", null },
            new object[] { 21, "Rio de Janeiro", 2020, "Teste", "NOVA" },
            new object[] { 1, "Rio de Janeiro", 10000, "Teste", "NOVA" },
            new object[] { 1, "Rio de Janeiro", -1, "Teste", "NOVA" },
            new object[] { 1, "", 2020, "Teste", "NOVA" },
            new object[] { 1, "Rio de Janeiro", 2020, "", "NOVA" }
        };
    }
}
