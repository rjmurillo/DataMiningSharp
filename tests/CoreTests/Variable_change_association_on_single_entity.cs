using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataMiningSharp.Core.DataClassifier.Causal;
using LightBDD;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataMiningSharp.Core.Tests
{
   

    [FeatureDescription(
        @"
As a Data Analyst
I can analyze causal relationships in my data
So that I can understand how different variables are linked
")]
    [TestClass]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class Variable_change_association_on_single_entity : FeatureFixture
    {
        private CausalDataClassifier<Model> _model;
        private IEnumerable<CausalDataClassifierResult> _patterns;

        [TestMethod]
        public void Successful_Pattern_Generation()
        {
            Runner.RunScenario(
                Given_a_minimal_amount_of_model_data,
                When_the_cause_variables_are_computed,
                When_the_association_rule_model_is_queried,
                Then_patterns_are_generated,
                There_are_two_patterns_generated

                );
        }

        [TestMethod]
        public void Successful_Pattern_Generation_With_Filters()
        {
            Runner.RunScenario(
                Given_a_minimal_amount_of_model_data,
                When_the_cause_variables_are_computed,
                When_the_association_model_is_queried_that_filters_thresholds_below_99,
                There_is_one_pattern_generated,
                There_is_only_one_pattern_with_threshold_above_99);
        }
    }

    public class Model
    {
        public double Duration { get; set; }

        public bool Indicator { get; set; }
    }
}