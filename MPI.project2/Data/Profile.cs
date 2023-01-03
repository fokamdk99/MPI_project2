namespace MPI.project2.Data;

public class Profile
{
    public int ContentId { get; set; }
    public int ProfileId { get; set; }
    public float Size { get; set; } // unit: GB
    public int ProfileQuality { get; set; }

    public Profile(int contentId,
        int profileId, 
        float size, 
        int profileQuality)
    {
        ContentId = contentId;
        ProfileId = profileId;
        Size = size;
        ProfileQuality = profileQuality;
    }
}