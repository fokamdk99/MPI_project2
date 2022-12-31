using Microsoft.Extensions.DependencyInjection;
using MPI.project2.Data;
using MPI.project2.Erlang;
using MPI.project2.FileReader;
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
                .AddTransient<IFileHandler, FileHandler>();

            var serviceProvider = services.BuildServiceProvider();

            var videoDimensioningMethods = serviceProvider.GetRequiredService<IVideoDimensioning>();

            var data = new General();
            videoDimensioningMethods.Run(data);

            return 0;
        }
    }
    
}



