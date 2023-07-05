using Microsoft.ML.Trainers;
using System.Collections;
using System.Collections.Generic;
using System.Formats.Tar;

namespace SCAI.Models
{
    public class ResultData
    {
        public float BestValue { get; set; }
        public string BestClass { get; set; }
        public Dictionary<string, float> AllResults { get; set; }

        public ResultData(Dictionary<string,float> results) 
        {
            if (results != null && results.Count > 0) 
            {
                AllResults = new Dictionary<string, float>();

                foreach (var item in results) 
                {
                    AllResults.Add(SkinCancers.GetCancer(item.Key), item.Value);
                }
                var max = AllResults.OrderByDescending(x => x.Value).First();
                BestValue = max.Value;
                BestClass = max.Key;
            }   
        }
    }
}
