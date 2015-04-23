using System.Reflection;

namespace DataMiningSharp.Core.DataClassifier.Causal
{
    public class DataClassifierFilterCauseVariable : IDataClassifierFilter<ICausalDataClassifierResult>
    {
        public DataClassifierFilterCauseVariable(PropertyInfo forbiddenVariable)
        {
            ForbiddenVariable = forbiddenVariable;
        }

        public bool IsSatisfied(ICausalDataClassifierResult rule)
        {
            return !ForbiddenVariable.Equals(rule.CauseVariable);
        }

        private PropertyInfo ForbiddenVariable { get; set; }
    }
}