using scb_equipamentos.Models.Enum;

namespace scb_equipamentos.Extensions
{
    public static class StatusBicicletaExtensions
    {
        private static Dictionary<string, StatusBicicleta> mapa = new Dictionary<string, StatusBicicleta> {
            {"NOVA", StatusBicicleta.NOVA},
            {"DISPONIVEL", StatusBicicleta.DISPONIVEL},
            {"EM_USO", StatusBicicleta.EM_USO},
            {"REPARO_SOLICITADO", StatusBicicleta.REPARO_SOLICITADO},
            {"EM_REPARO", StatusBicicleta.EM_REPARO},
            {"APOSENTADA", StatusBicicleta.APOSENTADA},
            {"EXCLUIDA", StatusBicicleta.EXCLUIDA}
         };


        public static string ParaStringBicicleta(this StatusBicicleta status)
        {

            return mapa.First(c => c.Value == status).Key;
        }

        public static StatusBicicleta ParaValorBicicleta(this string texto)
        {
            return mapa.First(c => c.Key == texto).Value;
        }
    }
}
