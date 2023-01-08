using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MPI.project2.Data;
using MPI.project2.Erlang;
using MPI.project2.FileReader;
using MPI.project2.VideoDimensioningMethod;

namespace MPI.projekt2.Tests;

public class ErlangModelTests
{
    private IFileHandler? _fileHandler;
    private IErlangModel? _erlangModel;
    private IVideoDimensioning? _videoDimensioning;
    private General? Data { get; set; } 
    
    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services
            .AddTransient<IErlangModel, ErlangModel>()
            .AddTransient<IFileHandler, FileHandler>()
            .AddTransient<IVideoDimensioning, VideoDimensioning>();

        var serviceProvider = services.BuildServiceProvider();

        _fileHandler = serviceProvider.GetRequiredService<IFileHandler>();
        _erlangModel = serviceProvider.GetRequiredService<IErlangModel>();
        _videoDimensioning = serviceProvider.GetRequiredService<IVideoDimensioning>();
        Data = _fileHandler.ReadDataFromFile();
        Data.Lambda = 1;
    }

    [Test]
    public void CalculateBlockingProbabilities()
    {
        Data!.X = GenerateTestX(Data.Profiles.Count);
        var methodInfo = typeof(VideoDimensioning).GetMethod("CalculateTraffic",
            BindingFlags.NonPublic | BindingFlags.Static);
        object[] parameters = { Data };
        var traffic = methodInfo!.Invoke(_videoDimensioning!, parameters);
        Assert.That(traffic, Is.Not.Null);
        _erlangModel!.SetErlangModel((decimal)0.99, Convert.ToDecimal(traffic!.ToString()));
        var blockingProbabilities = _erlangModel.CalculateBlockingProbabilities();
        Assert.That(blockingProbabilities.Last().Item1, Is.LessThan(1 - 0.99f));
    }
    
    private static IEnumerable<short> GenerateTestX(int numberOfProfiles)
    {
        var random = new Random();

        return Enumerable.Range(1, numberOfProfiles)
            .Select(i => Convert.ToInt16(random.Next(0, 2))).ToList();
    }
}