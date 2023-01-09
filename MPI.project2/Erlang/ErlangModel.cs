namespace MPI.project2.Erlang;

public class ErlangModel : IErlangModel
{
    public List<Tuple<decimal, short>> BlockingProbabilities { get; set; }
    public decimal Eta { get; set; }
    public decimal Traffic { get; set; }

    public ErlangModel()
    {
        BlockingProbabilities = new List<Tuple<decimal, short>> { new (1, 0) };
    }
    
    public ErlangModel(decimal eta, decimal traffic) : this()
    {
        Eta = eta;
        Traffic = traffic;
    }

    public void SetErlangModel(decimal eta, decimal traffic)
    {
        Eta = eta;
        Traffic = traffic;
        BlockingProbabilities.Clear();
        BlockingProbabilities.Add(new Tuple<decimal, short>(1, 0));
    }
    
    public List<Tuple<decimal, short>> CalculateBlockingProbabilities(short round = 1) // begin recursion by passing round = 1
    {
        var blockingProbability = 
            Traffic * BlockingProbabilities.ElementAt(round - 1).Item1 / 
            (Traffic * BlockingProbabilities.ElementAt(round - 1).Item1 + round);
        
        BlockingProbabilities.Add(new Tuple<decimal, short>(blockingProbability, round));

        if (blockingProbability * 100 <= 100 - Eta)
        {
            return BlockingProbabilities;
        }

        return CalculateBlockingProbabilities(Convert.ToInt16(round + 1));
    }
}