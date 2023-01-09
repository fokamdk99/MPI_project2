using MPI.project2.Data;
using MPI.project2.Erlang;
using MPI.project2.FileReader;
using MPI.project2.Utilities;

namespace MPI.project2.VideoDimensioningMethod;

public class VideoDimensioning : IVideoDimensioning
{
    private readonly IErlangModel _erlang;
    private readonly IFileHandler _fileHandler;
    private readonly IPermutationsGenerator _permutationsGenerator;
    private GeneralResult _generalResult;
    public VideoDimensioning(IErlangModel erlang, 
        IFileHandler fileHandler, 
        IPermutationsGenerator permutationsGenerator)
    {
        _erlang = erlang;
        _fileHandler = fileHandler;
        _permutationsGenerator = permutationsGenerator;
        _generalResult = new GeneralResult();
    }

    private float CalculateCostFunction(General data)
    {
        float storageCost = 0;
        
        foreach(var item in data.Profiles.Select((value, index) => new {value, index}))
        {
            storageCost += (1 - data.X.ElementAt(item.index))  * item.value.Size;
        }

        storageCost *= data.Delta;
        
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
        _permutationsGenerator.SetPermutationsGenerator(data);
        //var permutations = new List<IEnumerable<short>> { GenerateTestX(data.Profiles.Count) };
        
        var permutations = GeneratePermutations(data);

        foreach (var permutation in permutations)
        {
            _generalResult = CalculateResult(data, permutation);
        }
        
        _generalResult.SetGeneralResult(data.Delta,
            data.Gamma,
            data.Eta,
            data.Lambda);
        
        _fileHandler.WriteResultsToFile(_generalResult, data);
    }

    private GeneralResult CalculateResult(General data, IEnumerable<short> permutation)
    {
        data.X = permutation.ToList();
        var traffic = CalculateTraffic(data);
        _erlang.SetErlangModel(data.Eta, traffic);
        var blockingProbabilities = _erlang.CalculateBlockingProbabilities();
        var blockingProbability = blockingProbabilities.Last();

        var erlangColumn =
            data.ErlangTable
                .SingleOrDefault(x => x.Accessibility == data.Accessibility)?.Traffic
                .SingleOrDefault(p => p.Item1 == blockingProbability.Item2);
            
        data.Ksi = blockingProbability.Item2;
        data.Zeta = erlangColumn!.Item2;

        var cost = CalculateCostFunction(data);

        var result = new Result(cost, erlangColumn.Item1, traffic, erlangColumn.Item2);
        return SaveResult(result, data);
    }

    private GeneralResult SaveResult(Result result, General data)
    {
        _generalResult.Results.Add(result);
        if (result.Cost < _generalResult.MinCost)
        {
            _generalResult.MinCost = result.Cost;
            _generalResult.BestSolution = data.X;
        }

        return _generalResult;
    }
    
    private static IEnumerable<short> GenerateTestX(int numberOfProfiles)
    {
        var random = new Random();

        return Enumerable.Range(1, numberOfProfiles)
            .Select(_ => Convert.ToInt16(random.Next(0, 2))).ToList();
    }

    private IEnumerable<IEnumerable<short>> GeneratePermutations(General data)
    {
        int[] possibleValues = { 0, 1 };
        var numberOfPossibleValues = possibleValues.Length;
        var numberOfProfiles = data.Profiles.Count;
        var permutations =
            _permutationsGenerator
                .GeneratePermutations(possibleValues, numberOfPossibleValues, numberOfProfiles);

        return permutations;
    }
}