namespace MPI.project2.Erlang;

public class ErlangModel : IErlangModel
{
    public List<Tuple<float, short>> BlockingProbabilities { get; set; }
    public float Eta { get; set; }
    public float Traffic { get; set; }

    public ErlangModel()
    {
        BlockingProbabilities = new List<Tuple<float, short>> { new (1, 0) };
    }
    
    public ErlangModel(float eta, float traffic) : this()
    {
        Eta = eta;
        Traffic = traffic;
    }

    public void SetErlangModel(float eta, float traffic)
    {
        Eta = eta;
        Traffic = traffic;
    }
    
    public List<Tuple<float, short>> CalculateBlockingProbabilities(short round) // begin recursion by passing round = 1
    {
        var blockingProbability = 
            Traffic * BlockingProbabilities.ElementAt(round - 1).Item1 / 
            (Traffic * BlockingProbabilities.ElementAt(round - 1).Item1 + BlockingProbabilities.ElementAt(round - 1).Item2);
        
        BlockingProbabilities.Add(new (blockingProbability, round));

        if (blockingProbability <= 1 - Eta)
        {
            return BlockingProbabilities;
        }

        return CalculateBlockingProbabilities(Convert.ToInt16(round + 1));
    }
}