namespace MPI.project2.Erlang;

public interface IErlangModel
{
    List<Tuple<float, short>> CalculateBlockingProbabilities(short round);
    void SetErlangModel(float eta, float traffic);
}