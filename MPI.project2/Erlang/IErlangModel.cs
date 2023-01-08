namespace MPI.project2.Erlang;

public interface IErlangModel
{
    List<Tuple<decimal, short>> CalculateBlockingProbabilities(short round = 1);
    void SetErlangModel(decimal eta, decimal traffic);
}