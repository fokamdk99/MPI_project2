namespace MPI.project2.Erlang;

public interface IErlangModel
{
    List<Tuple<float, short>> CalculateBlockingProbabilities(short round = 1);
    void SetErlangModel(float eta, float traffic);
}