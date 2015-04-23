namespace DataMiningSharp.Core.DataClassifier
{
    public interface IDataClassifierResult
    {
        string ToString();



        double Recall { get; }



        double F1Score { get; }

        int Occurences { get; }

        double Precision { get; }
    }
}