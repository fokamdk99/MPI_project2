using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MPI.project2.Data;
using MPI.project2.Erlang;
using MPI.project2.FileReader;
using MPI.project2.HeurisitcVideoDimenioning;
using MPI.project2.Utilities;
using MPI.project2.VideoDimensioningMethod;

namespace MPI.projekt2.Tests;

public class HeuristicDimensioningTests
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
            .AddTransient<IVideoDimensioning, HeuristicDimensioning>()
            .AddTransient<IPermutationsGenerator, PermutationsGenerator>();

        var serviceProvider = services.BuildServiceProvider();

        _fileHandler = serviceProvider.GetRequiredService<IFileHandler>();
        _videoDimensioning = serviceProvider.GetRequiredService<IVideoDimensioning>();
        Data = _fileHandler.ReadDataFromFile();
    }

    [Test]
    public void RunHeuristic()
    {
        _videoDimensioning!.Run(Data!);
        Assert.Pass();
    }
}