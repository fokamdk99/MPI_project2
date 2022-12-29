namespace MPI.project2.Data;

public class Content
{
    public int ContentId { get; set; }
    public float ContentLength { get; set; } // unit: h
    public float Popularity { get; set; } // probability
    public List<Profile> Profiles { get; set; }
    
    public Content()
    {
        Profiles = new List<Profile>();
    }

    public Content(int contentId, float contentLength, float popularity, List<Profile> profiles)
    {
        ContentId = contentId;
        ContentLength = contentLength;
        Popularity = popularity;
        Profiles = profiles;
    }
}