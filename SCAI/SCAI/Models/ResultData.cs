namespace SCAI.Models
{
    public class ResultData
    {
        public float BestValue { get; set; }
        public int BestClass { get; set; }
        public List<float> AllResults { get; set; }

        public ResultData(List<float> results) 
        {
            if (results != null && results.Count > 0) 
            {
                AllResults = results;
                for (int i = 0; i < AllResults.Count; i++)
                {
                    if (AllResults[i] > BestValue)
                    {
                        BestValue = AllResults[i];
                        BestClass = i;
                    }
                }
            }
        }
    }
}
