using MPI.project2.Data;
using MPI.project2.FileReader;
using MPI.project2.Utilities;
using MPI.project2.VideoDimensioningMethod;

namespace MPI.project2.HeurisitcVideoDimensioning;

public class HeuristicDimensioning : IVideoDimensioning
{
    private readonly IPermutationsGenerator _permutationsGenerator;
    private HeuristicResults _heuristicResults;
    private readonly IFileHandler _fileHandler;
    private GeneralResult _generalResult;

    public HeuristicDimensioning(IPermutationsGenerator permutationsGenerator, 
        IFileHandler fileHandler)
    {
        _heuristicResults = new HeuristicResults();
        _permutationsGenerator = permutationsGenerator;
        _fileHandler = fileHandler;
        _generalResult = new GeneralResult();
    }

    public void Run(General data)
    {
        _heuristicResults = DivideVideos(data);
        SimmulatedAnnealing(data);
        _generalResult.SetGeneralResult(data.Delta,
            data.Gamma,
            data.Eta,
            data.Lambda);
        
        _fileHandler.WriteHeuristicResultsToFile(_heuristicResults, _generalResult);
    }

    // przejedz przez wszystkie filmy i sprawdz co sie bardziej oplaca
    // przechowywanie czy transkodowanie z uwzglednieniem popularnosci, czyli
    // czas transkodowania * popularnosc tresci * koszt
    private void SimmulatedAnnealing(General data)
    {
        var storedProfiles = _heuristicResults.StoredProfiles;
        var transcodedProfiles = _heuristicResults.TranscodedProfiles;
        storedProfiles = storedProfiles.OrderBy(p => p.AnnealingCoefficient).ToList();
        foreach (var profile in storedProfiles)
        {
            var relatedContent = data.Contents.First(x => x.ContentId == profile.ContentId); 
            var contentLength = relatedContent.ContentLength;
            var popularity = relatedContent.Popularity;
            profile.AnnealingCoefficient = contentLength * popularity;
        }

        for (var i = storedProfiles.Count - 1; i >= 0; i--)
        {
            if (storedProfiles[i].StorageCost > storedProfiles[i].AnnealingCoefficient * data.Gamma)
            {
                transcodedProfiles.Add(storedProfiles.ElementAt(i));
                storedProfiles.RemoveAt(i);
            }
        }
        
        _heuristicResults.StoredProfiles.AddRange(_heuristicResults.HighestQualityProfiles);
        
        
    }

    // podziel wideo na dwie listy: przechowywania oraz transkodowania
    // koszt transkodowania: czas trwania * koszt
    // koszt przechowywania: rozmiar * koszt
    // uwzglednij najwyzsza jakosc, ktora ma byc przechowywana
    private HeuristicResults DivideVideos(General data)
    {
        var storedProfiles = new List<Profile>();
        var transcodedProfiles = new List<Profile>();

        var highestQualityProfileIds = _permutationsGenerator.GetHighestQualityProfiles(data);
        var highestQualityProfiles = data.Profiles
            .Where((x, index) => highestQualityProfileIds.Contains(index)).ToList();

        var remainingProfiles = data.Profiles.Except(highestQualityProfiles, new ProfileComparer());
        
        foreach (var profile in remainingProfiles)
        {
            var contentLength = data.Contents.First(x => x.ContentId == profile.ContentId).ContentLength;
            profile.StorageCost = data.Gamma * contentLength;
            profile.TranscodingCost = data.Delta * profile.Size;
            if (profile.TranscodingCost < profile.StorageCost)
            {
                transcodedProfiles.Add(profile);
                continue;
            }
            
            storedProfiles.Add(profile);
        }

        return new HeuristicResults
        {
            StoredProfiles = storedProfiles,
            TranscodedProfiles = transcodedProfiles,
            HighestQualityProfiles = highestQualityProfiles
        };
    }

    public class HeuristicResults
    {
        public HeuristicResults()
        {
            StoredProfiles = new List<Profile>();
            TranscodedProfiles = new List<Profile>();
            HighestQualityProfiles = new List<Profile>();
        }

        public List<Profile> StoredProfiles { get; set; }
        public List<Profile> TranscodedProfiles { get; set; }
        public List<Profile> HighestQualityProfiles { get; set; }
    }
    
    class ProfileComparer : IEqualityComparer<Profile>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(Profile x, Profile y)
        {

            //Check whether the compared objects reference the same data.
            if (object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.ProfileId == y.ProfileId;
        }

        public int GetHashCode(Profile profile)
        {
            //Check whether the object is null
            if (object.ReferenceEquals(profile, null)) return 0;

            //Get hash code for the Name field if it is not null.
            var hashProfileName = profile.ProfileId.GetHashCode();

            //Get hash code for the Code field.
            var hashContentCode = profile.ContentId.GetHashCode();

            //Calculate the hash code for the product.
            return hashProfileName ^ hashContentCode;
        }
    }
}