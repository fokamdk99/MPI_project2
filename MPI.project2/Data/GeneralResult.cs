using static System.Single;

namespace MPI.project2.Data;

public class GeneralResult
{
    public List<Result> Results { get; set; }
    public int Delta { get; set; } // koszt przechowywania 1 GB/h
    public int Gamma { get; set; } // koszt transkodowania 1 strumienia/h
    public decimal Eta { get; set; } // dostepnosc systemu w %, np. 99.9%
    public float Lambda { get; set; } // intensywnosc naplywu [liczba zgloszen/s]
    public float MinCost { get; set; }
    public IEnumerable<short> BestSolution { get; set; }

    public GeneralResult()
    {
        Results = new List<Result>();
        BestSolution = new List<short>();
        MinCost = PositiveInfinity;
    }

    public void SetGeneralResult(int delta,
        int gamma,
        decimal eta,
        float lambda)
    {
        Delta = delta;
        Gamma = gamma;
        Eta = eta;
        Lambda = lambda;
    }
}