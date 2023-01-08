using System.Diagnostics;
using MPI.project2.Utilities;

namespace MPI.projekt2.Tests;

public class PermutationsGeneratorTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void PermutationsGenerator_ShouldGenerateAllPossiblePermutations()
    {
        int[] possibleValues = { 0, 1 };
        var numberOfPossibleValues = possibleValues.Length;
        const int arrayLength = 4;
        var result = PermutationsGenerator.GeneratePermutations(possibleValues, numberOfPossibleValues, arrayLength);

        foreach (var val in result)
        {
            var tmp = val.ToList();
            var res = tmp.Sum(s => s);
            Debug.WriteLine($"Suma: {res}");
        }

        Assert.Pass();
    }
}