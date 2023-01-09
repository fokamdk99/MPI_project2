using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MPI.project2.Utilities;

namespace MPI.projekt2.Tests;

public class PermutationsGeneratorTests
{
    private IPermutationsGenerator _permutationsGenerator;
    
    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services
            .AddTransient<IPermutationsGenerator, PermutationsGenerator>();

        var serviceProvider = services.BuildServiceProvider();

        _permutationsGenerator = serviceProvider.GetRequiredService<IPermutationsGenerator>();
    }

    [Test]
    public void PermutationsGenerator_ShouldGenerateAllPossiblePermutations()
    {
        int[] possibleValues = { 0, 1 };
        var numberOfPossibleValues = possibleValues.Length;
        const int arrayLength = 4;
        var result = 
            _permutationsGenerator.GeneratePermutations(possibleValues, numberOfPossibleValues, arrayLength);

        foreach (var val in result)
        {
            var tmp = val.ToList();
            var res = tmp.Sum(s => s);
            
            foreach (var item in tmp)
            {
                Debug.Write($"{item} ");
            }
            
            Debug.WriteLine($"Suma: {res}");
            
        }

        Assert.Pass();
    }
}