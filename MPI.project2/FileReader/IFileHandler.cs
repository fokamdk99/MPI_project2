using MPI.project2.Data;
using MPI.project2.HeurisitcVideoDimensioning;

namespace MPI.project2.FileReader;

public interface IFileHandler
{
    General ReadDataFromFile();
    void WriteResultsToFile(GeneralResult generalResult, General data);
    void WriteHeuristicResultsToFile(HeuristicDimensioning.HeuristicResults heuristicResults, GeneralResult generalResult);
}