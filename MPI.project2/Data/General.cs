using MPI.Project2.Data;

namespace MPI.project2.Data;

public class General
{
    public int Delta { get; set; } // koszt przechowywania 1 GB/h
    public int Gamma { get; set; } // koszt transkodowania 1 strumienia/h
    public decimal Eta { get; set; } // dostepnosc systemu w %, np. 99.9%
    public short Ksi { get; set; } // liczba urzadzen dla zalozonej dostepnosci eta 
    public float Zeta { get; set; } // ruch z tabeli Erlanga dla danego wiersza przy zalozonej dostepnosci 
    public IEnumerable<short> X { get; set; } // rozwiazanie
    public float Lambda { get; set; } // intensywnosc naplywu [liczba zgloszen/s]
    public Accessibility Accessibility { get; set; }
    public List<ErlangTraffic> ErlangTable { get; set; }

    public List<Content> Contents { get; set; }
    public List<Profile> Profiles { get; set; }

    public General()
    {
        X = new List<short>();
        Contents = new List<Content>();
        Profiles = new List<Profile>();
        ErlangTable = new List<ErlangTraffic>();
    }
}