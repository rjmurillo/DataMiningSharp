using System.Reflection;

namespace DataMiningSharp.Core.DataClassifier.Causal
{
    public class CausalDataClassifierResult : DataClassifierResult, ICausalDataClassifierResult
    {
        public CausalDataClassifierResult()
        {

        }

        public CausalDataClassifierResult(
            PropertyInfo causeVariable,
            object causeValue,
            PropertyInfo effectVariable,
            object effectValue,
            int occurences,
            double precision,
            double recall)
        {
            CauseVariable = causeVariable;
            CauseValue = causeValue;
            EffectVariable = effectVariable;
            EffectValue = effectValue;
            Precision = precision;
            Recall = recall;
            Occurences = occurences;
        }

        public object CauseValue { get;  set; }

        public PropertyInfo CauseVariable { get;  set; }

        public PropertyInfo EffectVariable { get;  set; }

        public object EffectValue { get;  set; }

        public override string ToString()
        {
            return string.Format(
                "{0} = {1} => {2} = {3}; with {4} occurrences, precision={5}, recall={6}, F-Score={7}",
                CauseVariable.Name,
                CauseValue,
                EffectVariable.Name,
                EffectValue,
                Occurences,
                Precision,
                Recall,
                F1Score);
        }
    }
}