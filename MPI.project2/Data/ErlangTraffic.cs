using MPI.Project2.Data;

namespace MPI.project2.Data;

public class ErlangTraffic
{
    public Accessibility Accessibility { get; set; }
    public List<Tuple<short, float>> Traffic { get; set; } // liczba urzadzen, ruch

    public ErlangTraffic()
    {
        Traffic = new List<Tuple<short, float>>();
    }

    public ErlangTraffic(Accessibility accessibility) : this()
    {
        Accessibility = accessibility;
    }
}