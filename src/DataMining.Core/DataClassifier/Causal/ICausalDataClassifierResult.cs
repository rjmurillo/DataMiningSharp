using System.Reflection;

namespace DataMiningSharp.Core.DataClassifier.Causal
{
    public interface ICausalDataClassifierResult : IDataClassifierResult
    {
        object CauseValue { get; set; }

        PropertyInfo CauseVariable { get; set; }
        PropertyInfo EffectVariable { get; set; }

        object EffectValue { get; set; }
    }
}