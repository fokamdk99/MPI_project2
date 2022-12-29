namespace MPI.project2.Data;

public class General
{
    public int Delta { get; set; } // koszt przechowywania 1 GB/h
    public int Gamma { get; set; } // koszt transkodowania 1 strumienia/h
    public List<short> Epsilon { get; set; } // wybrany wiersz z tabeli Erlanga
    public int Eta { get; set; } // dostepnosc systemu w %, np. 99.9%
    public List<short> Ksi { get; set; } // liczba urzadzen dla zalozonej dostepnosci eta 
    public int Zeta { get; set; } // ruch z tabeli Erlanga dla danego wiersza przy zalozonej dostepnosci 
    public List<short> X { get; set; } // rozwiazanie
    public int J { get; set; } // liczba urzadzen transkodujacych
    public int Lambda { get; set; } // intensywnosc naplywu [liczba zgloszen/s]
    public int Mi { get; set; } // sredni czas obslugi zgloszenia [s]

    public List<Content> Contents { get; set; }

    public General()
    {
        Epsilon = new List<short>();
        Ksi = new List<short>();
        X = new List<short>();
        Contents = new List<Content>();
    }
}