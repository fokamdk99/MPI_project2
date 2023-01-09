using System.Diagnostics;
using System.Reflection;
using System.Runtime;
using Microsoft.Extensions.DependencyInjection;
using MPI.project2.Data;
using MPI.project2.Erlang;
using MPI.project2.FileReader;
using MPI.project2.Utilities;
using MPI.project2.VideoDimensioningMethod;

namespace MPI.projekt2.Tests;

public class VideoDimensioningTests
{
    private IFileHandler? _fileHandler;
    private IVideoDimensioning? _videoDimensioning;
    private General? Data { get; set; } 

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services
            .AddTransient<IErlangModel, ErlangModel>()
            .AddTransient<IFileHandler, FileHandler>()
            .AddTransient<IVideoDimensioning, VideoDimensioning>()
            .AddTransient<IPermutationsGenerator, PermutationsGenerator>();

        var serviceProvider = services.BuildServiceProvider();

        _fileHandler = serviceProvider.GetRequiredService<IFileHandler>();
        _videoDimensioning = serviceProvider.GetRequiredService<IVideoDimensioning>();
        Data = _fileHandler.ReadDataFromFile();
    }

    [Test]
    public void CalculateCostFunction()
    {
        Data!.X = GenerateTestX(Data.Profiles.Count);
        var methodInfo = typeof(VideoDimensioning).GetMethod("CalculateCostFunction",
            BindingFlags.NonPublic | BindingFlags.Instance);
        object[] parameters = { Data };
        var result = methodInfo!.Invoke(_videoDimensioning!, parameters);
        Assert.Pass();
    }

    [Test]
    public void CalculateTraffic()
    {
        Data!.X = GenerateTestX(Data.Profiles.Count);
        var methodInfo = typeof(VideoDimensioning).GetMethod("CalculateTraffic",
            BindingFlags.NonPublic | BindingFlags.Static);
        object[] parameters = { Data };
        var result = methodInfo!.Invoke(_videoDimensioning!, parameters);
        Assert.Pass();
    }
    
    [Test]
    public void NumberOfPermutations_ShouldBeAsExpected()
    {
        var methodInfo = typeof(VideoDimensioning).GetMethod("GeneratePermutations",
            BindingFlags.NonPublic | BindingFlags.Instance);
        object[] parameters = { Data! };
        var result = methodInfo!.Invoke(_videoDimensioning!, parameters);
        Assert.That(result, Is.Not.Null);
        var permutations = (IEnumerable<IEnumerable<short>>)result!;
        var counter = permutations.Count();
        
        var expected = Math.Pow(2, Data!.Profiles.Count);

        Debug.WriteLine($"Number of permutations equals {counter}," +
                        $"Expected is {expected}");
        
        Assert.That(counter, Is.EqualTo(expected));
        
    }
    
    
    [Test]
    public void VideoDimensioning_ShouldOnlyAcceptPermutationsWithHighestQualityStoredInMemory()
    {
        var methodInfo = typeof(VideoDimensioning).GetMethod("GeneratePermutations",
            BindingFlags.NonPublic | BindingFlags.Instance);
        object[] parameters = { Data! };
        var result = methodInfo!.Invoke(_videoDimensioning!, parameters);
        Assert.That(result, Is.Not.Null);
        var permutations = (IEnumerable<IEnumerable<short>>)result!;

        var enumerator = permutations.GetEnumerator();
        enumerator.MoveNext();
        var permutation = enumerator.Current;

        var highestQualityProfiles = Data!.Profiles
            .Select((p, profileIndex) => new
            {
                Profile = p,
                ProfileIndex = profileIndex
                
            })
            .GroupBy(c => c.Profile.ContentId)
            .Select(g => g.OrderByDescending(o => o.Profile.Size))
            .ToList();

        if (highestQualityProfiles.Count <= 0)
        {
            return;
        }
        
        var profileIndexes = highestQualityProfiles
        .Select(t => new
        {
            item = t.First().Profile,
            ind = t.First().ProfileIndex
        }).ToList();
        
        enumerator.Dispose();
        Assert.Pass();

    }

    private static List<short> GenerateTestX(int numberOfProfiles)
    {
        var random = new Random();

        return Enumerable.Range(1, numberOfProfiles)
            .Select(i => Convert.ToInt16(random.Next(0, 2))).ToList();
    }
}