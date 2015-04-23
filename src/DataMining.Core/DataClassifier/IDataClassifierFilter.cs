namespace DataMiningSharp.Core.DataClassifier
{
    public interface IDataClassifierFilter<in TClassifierResult>
        where TClassifierResult : IDataClassifierResult
    {
        bool IsSatisfied(TClassifierResult result);
    }
}