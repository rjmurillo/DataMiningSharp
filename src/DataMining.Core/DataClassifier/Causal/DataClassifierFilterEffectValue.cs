namespace DataMiningSharp.Core.DataClassifier.Causal
{
    public class DataClassifierFilterEffectValue : IDataClassifierFilter<ICausalDataClassifierResult>
    {
        public DataClassifierFilterEffectValue(object effectValue)
        {
            EffectValue = effectValue;
        }

        private object EffectValue { get; set; }

        public bool IsSatisfied(ICausalDataClassifierResult rule)
        {
            return rule.EffectValue.Equals(EffectValue);
        }
    }
}