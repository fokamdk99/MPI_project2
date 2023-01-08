using MPI.project2.Data;
using MPI.project2.Erlang;
using MPI.project2.Utilities;

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

        //var transcodingCost = data.Epsilon.Select((t, i) => t * data.Ksi.ElementAt(i)).Sum();
        float transcodingCost = data.Ksi;
        transcodingCost *= data.Gamma;
        
        return storageCost + transcodingCost;
    }

    private static decimal CalculateTraffic(General data)
    {
        var traffic = data.Profiles.Select((p, i) => 
            data.X.ElementAt(i) * 
            data.Contents.Single(c => c.ContentId == p.ContentId).Popularity * 
            data.Contents.Single(ct => ct.ContentId == p.ContentId).ContentLength).Sum();
        
        return (decimal)(data.Lambda * traffic);
    }

    public void Run(General data)
    {
        var permutations = new List<IEnumerable<short>> { GenerateTestX(data.Profiles.Count) };
        //var permutations = GeneratePermutations(data);

        foreach (var permutation in permutations)
        {
            data.X = permutation.ToList();
            var traffic = CalculateTraffic(data);
            _erlang.SetErlangModel(data.Eta, traffic);
            var blockingProbabilities = _erlang.CalculateBlockingProbabilities();
            var result = blockingProbabilities.Last();
            Console.WriteLine($"Number of devices needed for the system to be available {data.Eta}% of the time " +
                              $"is {result.Item2}. Calculated blocking probability is " +
                              $"{result.Item1}");

            var erlangColumn =
                data.ErlangTable
                    .SingleOrDefault(x => x.Accessibility == data.Accessibility)?.Traffic
                    .SingleOrDefault(p => p.Item1 == result.Item2);
            
            data.Ksi = result.Item2;
            data.Zeta = erlangColumn!.Item2;

            var cost = CalculateCostFunction(data);
            
            Console.WriteLine($"Calculated cost function is {cost}");
            Console.WriteLine($"Erlang column: {erlangColumn.Item1}, {erlangColumn.Item2}");
            Console.WriteLine($"Calculated traffic: {traffic}");
        }
    }
    
    private static IEnumerable<short> GenerateTestX(int numberOfProfiles)
    {
        var random = new Random();

        return Enumerable.Range(1, numberOfProfiles)
            .Select(i => Convert.ToInt16(random.Next(0, 2))).ToList();
    }

    private static IEnumerable<IEnumerable<short>> GeneratePermutations(General data)
    {
        int[] possibleValues = { 0, 1 };
        var numberOfPossibleValues = possibleValues.Length;
        var numberOfProfiles = data.Profiles.Count;
        var permutations =
            PermutationsGenerator
                .GeneratePermutations(possibleValues, numberOfPossibleValues, numberOfProfiles);

        return permutations;
    }
}