using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataMiningSharp.Core
{
    public class VariableDomain<T, TValueKey> : IEnumerable<TValueKey>
    {
        public VariableDomain(IEnumerable<T> entities, Func<T, object> valueExtractor)
        {
            Entities = entities;
            ValueExtractor = valueExtractor;
            ComputeDomain();
        }

        private void AddEntry(TValueKey key, IEnumerable<T> entities)
        {
            if (MappingOfEntitiesByDomainValue.ContainsKey(key))
            {
                MappingOfEntitiesByDomainValue[key].AddRange(entities);
            }
            else
            {
                MappingOfEntitiesByDomainValue.Add(key, entities.ToList());
            }
        }

        private void ComputeDomain()
        {
            MappingOfEntitiesByDomainValue = new NullableDictionary<TValueKey, List<T>>();
            foreach (var grouping in Entities.GroupBy(entity => ValueExtractor(entity)))
            {
                if (grouping.Key is string)
                {
                    AddEntry((TValueKey)grouping.Key, grouping);
                }
                else
                {
                    var enumerable = grouping.Key as IEnumerable;
                    if (enumerable != null)
                    {
                        var key = enumerable;
                        foreach (var obj2 in key)
                        {
                            AddEntry((TValueKey)obj2, grouping);
                        }
                    }
                    else
                    {
                        AddEntry((TValueKey)grouping.Key, grouping);
                    }
                }
            }
        }

        public IEnumerable<T> GetEntities(TValueKey domainValue)
        {
            return MappingOfEntitiesByDomainValue[domainValue];
        }

        public IEnumerator<TValueKey> GetEnumerator()
        {
            return new DomainEnumerator(this);
        }

        public IEnumerable<T> GetRelevantEntities(TValueKey value)
        {
            return MappingOfEntitiesByDomainValue[value];
        }

        public void SortAsssociatedEntities(TValueKey domainValue, Comparison<T> sortCriteria)
        {
            MappingOfEntitiesByDomainValue[domainValue].Sort(sortCriteria);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> Entities { get; set; }

        private NullableDictionary<TValueKey, List<T>> MappingOfEntitiesByDomainValue { get; set; }

        private Func<T, object> ValueExtractor { get; set; }

        private class DomainEnumerator : IEnumerator<TValueKey>
        {
            private Dictionary<TValueKey, List<T>>.Enumerator _enumerator;

            public DomainEnumerator(VariableDomain<T, TValueKey> variableDomain)
            {
                VariableDomain = variableDomain;
                _enumerator = VariableDomain.MappingOfEntitiesByDomainValue.GetEnumerator();
            }

            public void Dispose()
            {
                VariableDomain = null;
                _enumerator.Dispose();
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator = VariableDomain.MappingOfEntitiesByDomainValue.GetEnumerator();
            }

            public TValueKey Current
            {
                get
                {
                    return _enumerator.Current.Key;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            private VariableDomain<T, TValueKey> VariableDomain { get; set; }
        }
    }
}