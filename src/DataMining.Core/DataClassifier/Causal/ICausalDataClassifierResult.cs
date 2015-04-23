using System.Reflection;

namespace DataMiningSharp.Core.DataClassifier.Causal
{
    public interface ICausalDataClassifierResult : IDataClassifierResult
    {
        object CauseValue { get; }

        PropertyInfo CauseVariable { get; }
        PropertyInfo EffectVariable { get; }

        object EffectValue { get; }
    }
}