using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DataMiningSharp.Core.DataClassifier.Causal
{
    public class CausalDataClassifier<TEntity, TClassifierResult> where TClassifierResult : ICausalDataClassifierResult, new()
    {
        public CausalDataClassifier()
        {
        }

        public CausalDataClassifier(IEnumerable<TEntity> entities, PropertyInfo targetVariable)
        {
            Entities = entities;
            TargetVariable = targetVariable;
        }

        public CausalDataClassifier(IEnumerable<TEntity> entities, string targetVariable)
            : this(entities, typeof(TEntity).GetProperty(targetVariable))
        {
        }

        public IEnumerable<TEntity> Entities { get; set; }

        public PropertyInfo TargetVariable { get; set; }

        private IEnumerable<Variable<TEntity, object>> CandidateCauseVariables { get; set; }

        public void ComputeCandidateCauseVariables()
        {
            Comparison<TEntity> sortCriteria = null;
            var collection = new Collection<Variable<TEntity, object>>();
            var infoArray = typeof(TEntity)
                                .GetProperties()
                                .Where(variable => !variable.Equals(TargetVariable) && variable.CanRead)
                                .ToArray();

            Func<TEntity, object> valueExtractor = null;
            foreach (var relevantProperty in infoArray)
            {
                if (valueExtractor == null)
                {
                    var property = relevantProperty;
                    valueExtractor = t => property.GetValue(t, null);
                }

                var variableDomain = new VariableDomain<TEntity, object>(Entities, valueExtractor);
                foreach (var domainValue in variableDomain)
                {
                    if (sortCriteria == null)
                    {
                        sortCriteria = delegate(TEntity x, TEntity y)
                        {
                            var valueOfX = TargetVariable.GetValue(x, null) as double?;
                            var valueOfY = TargetVariable.GetValue(y, null) as double?;

                            if ((valueOfX.GetValueOrDefault() < valueOfY.GetValueOrDefault()) && (valueOfX.HasValue & valueOfY.HasValue))
                            {
                                return 1;
                            }

                            if ((valueOfX.GetValueOrDefault() <= valueOfY.GetValueOrDefault()) || !(valueOfX.HasValue & valueOfY.HasValue))
                            {
                                return 0;
                            }
                            return -1;
                        };
                    }
                    variableDomain.SortAsssociatedEntities(domainValue, sortCriteria);
                }
                collection.Add(new Variable<TEntity, object>(relevantProperty, variableDomain));
            }
            CandidateCauseVariables = collection;
        }

        public IEnumerable<TEntity> GetRelevantEntities(CausalDataClassifierResult causalDataClassifierResult)
        {
            return GetRelevantEntities(causalDataClassifierResult.CauseVariable, causalDataClassifierResult.CauseValue);
        }

        public IEnumerable<TEntity> GetRelevantEntities(PropertyInfo property, object value)
        {
            var variable = CandidateCauseVariables.FirstOrDefault(v => v.Property.Equals(property));
            if (variable == null)
            {
                return null;
            }
            return variable.GetRelevantEntities(value);
        }

        public IEnumerable<TClassifierResult> Query(double minSupport, double minConfidence, IEnumerable<IDataClassifierFilter<TClassifierResult>> filters = null)
        {
            var entityCount = Entities.Count();

            Debug.Assert(minSupport > 0, "minSupport>0");
            Debug.Assert(minSupport <= entityCount, "minSupport <= entityCount");

            Debug.Assert(minConfidence >= 1, "minConfidence >= 1");
            Debug.Assert(minConfidence <= 100, "minConfidence <= 100");

            var collection = new Collection<TClassifierResult>();
            foreach (var variable in CandidateCauseVariables)
            {
                var property = variable.Property;
                var variableDomain = variable.VariableDomain;
                foreach (var domainValue in variableDomain)
                {
                    var source = variableDomain.GetEntities(domainValue).ToList();
                    var sourceCount = source.Count();
                    var maxOfEntityAndMinSupport = (int)Math.Ceiling(minSupport * entityCount);
                    var maxOfMinConfidenceAndSourceCount = (int)Math.Ceiling(minConfidence * sourceCount);
                    var requiredOccurrences = Math.Max(maxOfMinConfidenceAndSourceCount, maxOfEntityAndMinSupport);
                    if (requiredOccurrences <= sourceCount)
                    {
                        var recall = requiredOccurrences / ((double)sourceCount);
                        var precision = requiredOccurrences / ((double)entityCount);
                        var occurrences = requiredOccurrences;
                        var effectVariableThreshold = (double)TargetVariable.GetValue(source[requiredOccurrences - 1], null);
                        var result = new TClassifierResult
                        {
                            CauseVariable = property,
                            CauseValue = domainValue,
                            EffectVariable = TargetVariable,
                            EffectValue = effectVariableThreshold,
                            Occurences = occurrences,
                            Precision = precision,
                            Recall = recall
                        };

                        if (filters.AllSatisfied(result))
                        {
                            collection.Add(result);
                        }
                        else
                        {
                            Debug.WriteLine("Samples did not satisfy all filters.");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Not enough samples; need {0}, received {1}.", requiredOccurrences, sourceCount);
                    }
                }
            }
            return collection;
        }
    }
}