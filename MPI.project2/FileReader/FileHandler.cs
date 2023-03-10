using MPI.project2.Data;
using MPI.Project2.Data;
using MPI.project2.HeurisitcVideoDimensioning;

namespace MPI.project2.FileReader;

public class FileHandler : IFileHandler
{
    public string ContentsFile => "contents.csv";
    public string ProfilesFile => "profiles.csv";
    public string ErlangTableFile => "erlang_table.csv";
    public string GeneralDataFile => "general.csv";
    
    public General ReadDataFromFile()
    {
        var fileDirectory = GetFileDirectory();

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

    public void WriteHeuristicResultsToFile(HeuristicDimensioning.HeuristicResults heuristicResults, GeneralResult generalResult)
    {
        var fileDirectory = GetFileDirectory();
        var folder = 
            $"{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}/";
        var filePath = $"{fileDirectory}/results/heuristics/{folder}";
        SaveHeuristicResults(heuristicResults, filePath);
        SaveExperimentParameters(generalResult, filePath);
    }

    public void SaveHeuristicResults(HeuristicDimensioning.HeuristicResults heuristicResults, string filePath)
    {
        var log = "";

        log += "profileId;isTranscoded\n";
        
        foreach (var p in heuristicResults.StoredProfiles)
        {
            var tmp = p.ProfileId + ";" + "0" + "\n";
            log += tmp;
        }
        
        foreach (var p in heuristicResults.TranscodedProfiles)
        {
            var tmp = p.ProfileId + ";" + "1" + "\n";
            log += tmp;
        }
        
        log += "\n";

        const string fileName = "results.csv";

        WriteToFile(filePath, fileName, log);
    }
    public void WriteResultsToFile(GeneralResult generalResult, General data)
    {
        var fileDirectory = GetFileDirectory();
        var folder = 
            $"{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}/";
        var filePath = $"{fileDirectory}/results/{folder}";
        
        SaveResults(generalResult.Results, filePath);
        SaveExperimentParameters(generalResult, filePath);
        SaveBestSolution(generalResult.BestSolution, filePath);
    }

    private static void SaveBestSolution(IEnumerable<short> solution, string filePath)
    {
        var log = "";
        log += "index;value\n";
        foreach (var item in solution.Select((value, index) => new {value, index}))
        {
            log += $"{item.index};{item.value}\n";
        }

        const string fileName = "solution.csv";

        WriteToFile(filePath, fileName, log);
    }
    private static void SaveExperimentParameters(GeneralResult generalResult, string filePath)
    {
        var log = "";

        log += $"Delta;{generalResult.Delta}\n";
        log += $"Gamma;{generalResult.Gamma}\n";
        log += $"Eta;{generalResult.Eta}\n";
        log += $"Lambda;{generalResult.Lambda}\n";

        const string fileName = "parameters.csv";

        WriteToFile(filePath, fileName, log);
    }
    
    private static void SaveResults(List<Result> list, string filePath)
    {
        var log = "";

        log += "Cost;NumberOfDevices;CalculatedTraffic;ErlangTraffic\n";
        
        foreach (var t in list)
        {
            var tmp = t.Cost + ";" + t.NumberOfDevices + ";" + t.CalculatedTraffic + ";" + t.ErlangTraffic + "\n";
            log += tmp;
        }
        log += "\n";

        const string fileName = "results.csv";

        WriteToFile(filePath, fileName, log);
    }
    
    private static void WriteToFile(string filePath, string fileName, string log)
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        
        var file = filePath + fileName;
        if (!File.Exists(file))
        {
            using var sw = File.CreateText(file);
            sw.WriteLine(log);
        }
        else
        {
            using var sw = File.AppendText(file);
            sw.WriteLine(log);
        }
    }

    private static string GetFileDirectory()
    {
        var workingDirectory = Directory.GetCurrentDirectory();
        var fileDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
        if (fileDirectory == null)
        {
            throw new DirectoryNotFoundException(
                $"Could not find project directory based on working directory: {workingDirectory}");
        }

        return fileDirectory;
    }

    private static IEnumerable<string> ReadCsv(string filePath)
    {
        return File.ReadLines(filePath);
    }

    private static General ReadGeneralData(IEnumerable<string> lines)
    {
        var values = lines.ElementAt(1).Split(';');
        var data = new General
        {
            Delta = Convert.ToInt32(values.ElementAt(0)),
            Gamma = Convert.ToInt32(values.ElementAt(1)),
            Lambda = float.Parse(values.ElementAt(2))
        };

        var blockingProbability = Convert.ToDecimal(values.ElementAt(3));

        data.Eta = 100 - blockingProbability;
        data.Accessibility = MapAccessibility(blockingProbability);

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
                Convert.ToInt32(values.ElementAt(1)),
                float.Parse(values.ElementAt(2)),
                Convert.ToInt32(values.ElementAt(3)));
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
            data.Add(new ErlangTraffic(MapAccessibility(probability)));
        }
        
        return data;
    }

    private static Accessibility MapAccessibility(float eta)
    {
        return eta switch
        {
            0.01f => Accessibility.P9999,
            0.05f => Accessibility.P9995,
            0.1f => Accessibility.P999,
            0.5f => Accessibility.P995,
            1f => Accessibility.P99,
            2f => Accessibility.P98,
            5f => Accessibility.P95,
            10f => Accessibility.P90,
            _ => throw new ArgumentException($"{eta} could not be mapped")
        };
    }
    
    private static Accessibility MapAccessibility(decimal eta)
    {
        return eta switch
        {
            (decimal)0.01 => Accessibility.P9999,
            (decimal)0.05 => Accessibility.P9995,
            (decimal)0.1 => Accessibility.P999,
            (decimal)0.5 => Accessibility.P995,
            1 => Accessibility.P99,
            2 => Accessibility.P98,
            5 => Accessibility.P95,
            10 => Accessibility.P90,
            _ => throw new ArgumentException($"{eta} could not be mapped")
        };
    }
}