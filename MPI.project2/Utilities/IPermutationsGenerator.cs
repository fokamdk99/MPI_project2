using MPI.project2.Data;

namespace MPI.project2.Utilities;

public interface IPermutationsGenerator
{
    IEnumerable<IEnumerable<short>> GeneratePermutations(int[] possibleValues, int numberOfPossibleValues,
        int arrayLength);

    void SetPermutationsGenerator(General data);
    IEnumerable<int> GetHighestQualityProfiles(General data);
}