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

        public int Occurences { get; private set; }
        public double Precision { get; private set; }
        public double Recall { get; private set; }
    }
}