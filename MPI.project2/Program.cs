using Microsoft.Extensions.DependencyInjection;
using MPI.project2.Erlang;
using MPI.project2.FileReader;
using MPI.project2.Utilities;
using MPI.project2.VideoDimensioningMethod;

namespace MPI.project2
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var services = new ServiceCollection();
            services
                .AddTransient<IErlangModel, ErlangModel>()
                .AddTransient<IVideoDimensioning, VideoDimensioning>()
                .AddTransient<IFileHandler, FileHandler>()
                .AddTransient<IPermutationsGenerator, PermutationsGenerator>();

            var serviceProvider = services.BuildServiceProvider();

            var fileHandler = serviceProvider.GetRequiredService<IFileHandler>();
            var videoDimensioningMethods = serviceProvider.GetRequiredService<IVideoDimensioning>();

            var data = fileHandler.ReadDataFromFile();
            videoDimensioningMethods.Run(data);

            return 0;
        }
    }
    
}



