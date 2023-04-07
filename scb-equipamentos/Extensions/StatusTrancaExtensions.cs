using scb_equipamentos.Models.Enum;

namespace scb_equipamentos.Extensions
{
    public static class StatusTrancaExtensions
    {
        private static Dictionary<string, StatusTranca> mapa = new Dictionary<string, StatusTranca> {
            {"NOVA", StatusTranca.NOVA},
            {"LIVRE", StatusTranca.LIVRE},
            {"OCUPADA", StatusTranca.OCUPADA},
            {"REPARO_SOLICITADO", StatusTranca.REPARO_SOLICITADO},
            {"EM_REPARO", StatusTranca.EM_REPARO},
            {"APOSENTADA", StatusTranca.APOSENTADA},
            {"EXCLUIDA", StatusTranca.EXCLUIDA}
         };


        public static string ParaStringTranca(this StatusTranca status)
        {

            return mapa.First(c => c.Value == status).Key;
        }

        public static StatusTranca ParaValorTranca(this string texto)
        {
            return mapa.First(c => c.Key == texto).Value;
        }
    }
}
