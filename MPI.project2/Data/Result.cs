namespace MPI.project2.Data;

public class Result
{
    public float Cost { get; set; }
    public short NumberOfDevices { get; set; }
    public decimal CalculatedTraffic { get; set;}
    public float ErlangTraffic { get; set; }

    public Result(float cost,
        short numberOfDevices,
        decimal calculatedTraffic,
        float erlangTraffic)
    {
        Cost = cost;
        NumberOfDevices = numberOfDevices;
        CalculatedTraffic = calculatedTraffic;
        ErlangTraffic = erlangTraffic;
    }
}