using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataMiningSharp.Core.DataClassifier.Causal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataMiningSharp.Core.Tests
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class Variable_change_association_on_single_entity
    {
        private void PrintPatterns()
        {
            Debug.WriteLine("\r\nFound {0} pattern(s)", _patterns.Count());
            foreach (var r in _patterns)
            {
                Debug.WriteLine(" * " + r);
            }
        }

        private void Given_a_minimal_amount_of_model_data()
        {
            _model.Entities = new[]
            {
                new Model {Duration = 99d, Indicator = true},
                new Model {Duration = 99d, Indicator = true},
                new Model {Duration = 99d, Indicator = true},
                 new Model {Duration = 99d, Indicator = false},
                new Model {Duration = 87d, Indicator = false}
            };
        }

        private void When_the_cause_variables_are_computed()
        {
            _model.ComputeCandidateCauseVariables();
        }

        private void When_the_association_rule_model_is_queried()
        {
            // Minimum one entity and 1% confidence with no filters
            _patterns = _model.Query(1 / (double)_model.Entities.Count(), 1);
        }

        private void Then_patterns_are_generated()
        {
            Assert.IsTrue(_patterns.Any());
        }

        private void There_is_one_pattern_generated()
        {
            Assert.IsTrue(_patterns.Count() == 1);
        }

        private void There_are_two_patterns_generated()
        {
            Assert.IsTrue(_patterns.Count() == 2);
        }

        private void There_is_only_one_pattern_with_threshold_above_99()
        {
            Assert.IsTrue((_patterns.Single().EffectValue.Equals(99d)));
        }

        private void When_the_association_model_is_queried_that_filters_thresholds_below_99()
        {
            _patterns = _model.Query(1 / (double)_model.Entities.Count(), 1, new[] { new DataClassifierFilterEffectValue(99d) });
        }

        [TestInitialize]
        public void SetUp()
        {
            _model = new CausalDataClassifier<Model, CausalDataClassifierResult>
            {
                TargetVariable = typeof(Model).GetProperty("Duration")
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            PrintPatterns();
        }
    }
}
