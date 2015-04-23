namespace DataMiningSharp.Core.DataClassifier
{
    public interface IDataClassifierResult
    {
        string ToString();



        double Recall { get; set; }


        double F1Score { get; }

        int Occurences { get; set; }

        double Precision { get; set; }
    }
}