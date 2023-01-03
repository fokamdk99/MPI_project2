using MPI.project2.Data;
using MPI.Project2.Data;

namespace MPI.project2.FileReader;

public class FileHandler : IFileHandler
{
    public string ContentsFile => "contents.csv";
    public string ProfilesFile => "profiles.csv";
    public string ErlangTableFile => "erlang_table.csv";
    public string GeneralDataFile => "general.csv";
    
    public General ReadDataFromFile()
    {
        var workingDirectory = Directory.GetCurrentDirectory();
        var fileDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
        if (fileDirectory == null)
        {
            throw new DirectoryNotFoundException(
                $"Could not find project directory based on working directory: {workingDirectory}");
        }

        var filePath = $"{fileDirectory}/Files/";
        var data = ReadGeneralData(ReadCsv($"{filePath}{GeneralDataFile}"));
        var contents = ReadContents(ReadCsv($"{filePath}{ContentsFile}"));
        var profiles = ReadProfiles(ReadCsv($"{filePath}{ProfilesFile}"));
        var erlangTable = ReadErlangTable(ReadCsv($"{filePath}{ErlangTableFile}"));

        data.Contents = contents;
        data.Profiles = profiles;
        data.ErlangTable = erlangTable;
        
        return data;
    }

    private static IEnumerable<string> ReadCsv(string filePath)
    {
        return File.ReadLines(filePath);
    }

    private General ReadGeneralData(IEnumerable<string> lines)
    {
        var values = lines.ElementAt(1).Split(';');
        var data = new General();
        data.Delta = Convert.ToInt32(values.ElementAt(0));
        data.Gamma = Convert.ToInt32(values.ElementAt(1));

        return data;
    }
    
    private static List<Content> ReadContents(IEnumerable<string> lines)
    {
        var contents = new List<Content>();
        var data = lines.Skip(1);
        foreach (var line in data)
        {
            var values = line.Split(';');
            var content = new Content(Convert.ToInt32(values.ElementAt(0)),
                float.Parse(values.ElementAt(1)),
                    float.Parse(values.ElementAt(2)));
            contents.Add(content);
        }
        
        return contents;
    }
    
    private static List<Profile> ReadProfiles(IEnumerable<string> lines)
    {
        var profiles = new List<Profile>();
        var data = lines.Skip(1);
        foreach (var line in data)
        {
            var values = line.Split(';');
            var profile = new Profile(Convert.ToInt32(values.ElementAt(0)),
                float.Parse(values.ElementAt(1)),
                Convert.ToInt32(values.ElementAt(2)));
            profiles.Add(profile);
        }
        
        return profiles;
    }
    
    private static List<ErlangTraffic> ReadErlangTable(IEnumerable<string> lines)
    {
        var data = lines.ToList();
        var blockingProbabilities = data.ElementAt(0).Split(';').Skip(1).Select(float.Parse);
        var erlangTable = MapProbabilityToAccessibility(blockingProbabilities);
        foreach (var line in data.Skip(1))
        {
            var values = line.Split(';');

            var numberOfServers = Convert.ToInt16(values.ElementAt(0));
            var trafficList = values.Skip(1).Select(float.Parse);
            foreach (var traffic in trafficList.Select((val, index) => new {val, index}))
            {
                erlangTable.ElementAt(traffic.index).Traffic.Add(new Tuple<short, float>(numberOfServers, traffic.val));
            }
        }
        
        return erlangTable;
    }

    private static List<ErlangTraffic> MapProbabilityToAccessibility(IEnumerable<float> blockingProbabilities)
    {
        var data = new List<ErlangTraffic>();
        foreach (var probability in blockingProbabilities)
        {
            var accessibility = probability switch
            {
                0.01f => Accessibility.P9999,
                0.05f => Accessibility.P9995,
                0.1f => Accessibility.P999,
                0.5f => Accessibility.P995,
                1f => Accessibility.P99,
                2f => Accessibility.P98,
                5f => Accessibility.P95,
                10f => Accessibility.P90,
                _ => throw new ArgumentException($"{probability} could not be mapped")
            };

            data.Add(new ErlangTraffic(accessibility));
        }
        
        return data;
    }
}