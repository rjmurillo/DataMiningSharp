# DataMiningSharp

## Usage
There are two parts: the model containing your data for analysis, and the program that actually performs the analysis.

The implementation is currently limited to analysis within a single model.

When you query the classifier you will need to specify values for [precision](http://en.wikipedia.org/wiki/Positive_and_negative_predictive_values) and [recall](http://en.wikipedia.org/wiki/Sensitivity_and_specificity). In statistics, if the 
[null hypothesis](http://en.wikipedia.org/wiki/Null_hypothesis) is that all and only the relevant items are retreived, the absence of [type I](http://en.wikipedia.org/wiki/Type_I_and_type_II_errors#Type_I_error) and [type II](http://en.wikipedia.org/wiki/Type_I_and_type_II_errors#Type_II_error) errors 
corresponds respectively to maximum precision (no false positives) and maximum recall (no false negatives).
    
**Example**
The classifier is analyzing dogs in scenes from a video containing both cats and dogs.

The classifier identifies 7 dogs in a scene containing 9 dogs and some cats. If 4 of the identifications are 
correct, but the remaining 3 are actually cats, the program's precision is 4/7 while its recall is 4/9.
That is, the results contained 7 - 4 = 3 type I errors and 9 - 4 = 5 type II errors.

Precision can be seen as a measure of exactness or QUALITY, whereas recall is a measure of completeness or QUANTITY.
    
**TL;DR** - high precision means more relevant results than irrelevant, high recall means most relevant results.

Given the above example, we want to analyze the relationship between the scene number, 
the number of dogs found, and the number of cats found.
```
public class SceneAnalysis
{
  public property int Scene { get; set; }
  public property int DogCount { get; set; }
  public property int CatCount {get; set; }
}
```
Once you create your model definition, it will need to be populated with data. You can do this any way you would like. I've left the implementation of that blank in the example below.

Here is how you use the data classifier:
```
class Program
{
  static void Main()
  {
    var classifier = new CausalDataClassifier<SceneAnalysis, CausalDataClassifierResult>
    {
      TargetVariable = typeof(SceneAnalysis).GetProperty("DogCount");
    };
    
    classifier.Entities = ... /* Read from database, file, etc. into your Model */
    
    classifier.ComputeCandidateCauseVariables();
    
    // You're going to want to adjust these!
    var minimumPrecision = 1 / (double)classifier.Entities.Count();
    var minimumRecall = 1;
    
    var patterns = classifier.Query(minimumPrecision, minimumRecall);
    
    foreach(var pattern in patterns)
    {
      Console.WriteLine(pattern);
    }
  }
}
```
After executing the program the output will look something like this:
```
Scene = 1 => DogCount = 7; with 3 occurrences, precision=0.57, recall=0.44, F-Score=0.3174
```
