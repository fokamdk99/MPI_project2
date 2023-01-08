using System.Diagnostics;

namespace MPI.project2.Utilities;

public static class PermutationsGenerator
{
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
    
    public static IEnumerable<IEnumerable<short>> GeneratePermutations(int []possibleValues, int numberOfPossibleValues, int arrayLength)
    {
        for (var i = 0;
             i < (int)Math.Pow(numberOfPossibleValues, arrayLength); i++)
        {
            yield return ConvertBase(i, possibleValues, numberOfPossibleValues, arrayLength);
        }
    }
}