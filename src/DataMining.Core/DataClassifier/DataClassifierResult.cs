namespace DataMiningSharp.Core.DataClassifier
{
    public class DataClassifierResult : IDataClassifierResult
    {
        public double F1Score
        {
            get
            {
                // http://en.wikipedia.org/wiki/F1_score
                return 2.0 * ((Precision * Recall) / (Precision + Recall));
            }
        }

        public int Occurrences { get;  set; }
        public double Precision { get;  set; }
        public double Recall { get;  set; }
    }
}