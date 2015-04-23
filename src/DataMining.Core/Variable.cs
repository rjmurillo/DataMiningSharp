using System.Collections.Generic;
using System.Reflection;

namespace DataMiningSharp.Core
{
    internal class Variable<TEntity, TValueKey>
    {
        public Variable(PropertyInfo propertyInfo, VariableDomain<TEntity, TValueKey> variableDomain)
        {
            Property = propertyInfo;
            VariableDomain = variableDomain;
        }

        public IEnumerable<TEntity> GetRelevantEntities(TValueKey value)
        {
            return VariableDomain.GetRelevantEntities(value);
        }

        public PropertyInfo Property { get; private set; }

        public VariableDomain<TEntity, TValueKey> VariableDomain { get; private set; }
    }
}