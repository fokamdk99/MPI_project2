using Microsoft.Extensions.DependencyInjection;
using MPI.project2.FileReader;

namespace MPI.projekt2.Tests;

public class Tests
{
    private IFileHandler? _fileHandler;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services
            .AddTransient<IFileHandler, FileHandler>();

        var serviceProvider = services.BuildServiceProvider();

        _fileHandler = serviceProvider.GetRequiredService<IFileHandler>();
    }

    [Test]
    public void WhenInputFilesAreRead_MethodDoesNotThrowException()
    {
        Assert.DoesNotThrow(() => _fileHandler!.ReadDataFromFile());
    }
}