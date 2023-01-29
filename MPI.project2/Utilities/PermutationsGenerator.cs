using MPI.project2.Data;

namespace MPI.project2.Utilities;

public class PermutationsGenerator : IPermutationsGenerator
{
    private IEnumerable<int> HighestQualityProfiles { get; set; }
    public PermutationsGenerator()
    {
        HighestQualityProfiles = new List<int>();
    }

    public void SetPermutationsGenerator(General data)
    {
        HighestQualityProfiles = GetHighestQualityProfiles(data);
    }
    
    private static IEnumerable<short> ConvertBase(int n, IReadOnlyList<int> possibleValues,
        int numberOfPossibleValues, int arrayLength)
    {
        var results = new List<short>();
        
        for (var i = 0; i < arrayLength; i++)
        {
            results.Add(Convert.ToInt16(possibleValues[n % numberOfPossibleValues]));
            n /= numberOfPossibleValues;
        }
        
        return results;
    }
    
    public IEnumerable<IEnumerable<short>> GeneratePermutations(int []possibleValues, int numberOfPossibleValues, int arrayLength)
    {
        for (int i = 0;
             i < (int)Math.Pow(numberOfPossibleValues, arrayLength); i++)
        {
            var permutation = ConvertBase(i, possibleValues, numberOfPossibleValues, arrayLength).ToList();
            foreach (var index in HighestQualityProfiles)
            {
                permutation[index] = 0;
            }

            yield return permutation;
            
            //yield return ConvertBase(i, possibleValues, numberOfPossibleValues, arrayLength);
        }
    }
    
    public IEnumerable<int> GetHighestQualityProfiles(General data)
    {
        var highestQualityProfiles = data.Profiles
            .Select((p, profileIndex) => new
            {
                Profile = p,
                ProfileIndex = profileIndex
                
            })
            .GroupBy(c => c.Profile.ContentId)
            .Select(g => g.OrderByDescending(o => o.Profile.Size))
            .Select(t => t.First().ProfileIndex)
            .ToList();

        return highestQualityProfiles;
    }
}