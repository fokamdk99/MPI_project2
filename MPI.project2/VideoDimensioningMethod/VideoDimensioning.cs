using MPI.project2.Data;
using MPI.project2.Erlang;

namespace MPI.project2.VideoDimensioningMethod;

public class VideoDimensioning : IVideoDimensioning
{
    private readonly IErlangModel _erlang;

    public VideoDimensioning(IErlangModel erlang)
    {
        _erlang = erlang;
    }

    private float CalculateCostFunction(General data)
    {
        float storageCost = 0;
        
        foreach(var item in data.Profiles.Select((value, index) => new {value, index}))
        {
            storageCost += (1 - data.X.ElementAt(item.index))  * item.value.Size;
        }

        storageCost *= data.Delta;

        var transcodingCost = data.Epsilon.Select((t, i) => t * data.Ksi.ElementAt(i)).Sum();
        transcodingCost *= data.Gamma;
        
        return storageCost + transcodingCost;
    }

    private static float CalculateTraffic(General data)
    {
        var traffic = data.X.Select((x, i) => 
            x * data.Contents.ElementAt(i).Popularity * data.Contents.ElementAt(i).ContentLength).Sum();
        return data.Lambda * traffic;
    }

    public void Run(General data)
    {
        var traffic = CalculateTraffic(data);
        _erlang.SetErlangModel(data.Eta, traffic);
        var blockingProbabilities = _erlang.CalculateBlockingProbabilities(1);
        Console.WriteLine($"Number of devices needed to for the system to be available {data.Eta}% of the time " +
                          $"is {blockingProbabilities.Last().Item2}. Calculated blocking probability is " +
                          $"{blockingProbabilities.Last().Item1}");
    }
}