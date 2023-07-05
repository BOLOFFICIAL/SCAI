namespace SCAI.Models
{
    public class SkinCancers
    {
        private static Dictionary<string, string> Cancers = new Dictionary<string, string>()
        {
            {"akiec","Актинический кератоз и внутриэпителиальный карцинома / болезнь Боуэна"},
            {"bcc","Базалиома"},
            {"bkl","Доброкачественные кератозоподобные образования (солнечные лентиго / себорейные кератозы и лихено-подобные кератозы)"},
            {"df","Дерматофиброма"},
            {"mel","Меланома"},
            {"nv","Меланоцитовые невусы"},
            {"vasc","Сосудистые образования (ангиомы, ангиокератомы, пиогенные грануломы и кровотечения)"}
        };


        public static string GetCancer(string key)
        {
            return Cancers[key];
        }
    }
}
