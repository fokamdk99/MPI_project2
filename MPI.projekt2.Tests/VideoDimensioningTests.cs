using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MPI.project2.Erlang;
using MPI.project2.FileReader;
using MPI.project2.VideoDimensioningMethod;

namespace MPI.projekt2.Tests;

public class VideoDimensioningTests
{
    private IFileHandler? _fileHandler;
    private IVideoDimensioning? _videoDimensioning;

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
        _videoDimensioning = serviceProvider.GetRequiredService<IVideoDimensioning>();
    }

    [Test]
    public void CalculateCostFunction()
    {
        var data = _fileHandler!.ReadDataFromFile();
        data.X = GenerateTestX(data.Profiles.Count);
        var methodInfo = typeof(VideoDimensioning).GetMethod("CalculateCostFunction",
            BindingFlags.NonPublic | BindingFlags.Instance);
        object[] parameters = { data };
        var result = methodInfo!.Invoke(_videoDimensioning!, parameters);
        Assert.Pass();
    }

    private List<short> GenerateTestX(int numberOfProfiles)
    {
        var random = new Random();

        return Enumerable.Range(1, numberOfProfiles)
            .Select(i => Convert.ToInt16(random.Next(0, 2))).ToList();
    }
}