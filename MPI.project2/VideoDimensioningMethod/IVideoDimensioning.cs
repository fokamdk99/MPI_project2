using MPI.project2.Data;

namespace MPI.project2.VideoDimensioningMethod;

public interface IVideoDimensioning
{
    float CalculateCostFunction(General generalData);
    void Run(General data);
}