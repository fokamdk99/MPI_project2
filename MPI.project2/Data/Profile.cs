namespace MPI.project2.Data;

public class Profile
{
    public int ProfileId { get; set; }
    public float Size { get; set; } // unit: GB
    public int ProfileQuality { get; set; }

    public Profile(int profileId, 
        float size, 
        int profileQuality)
    {
        ProfileId = profileId;
        Size = size;
        ProfileQuality = profileQuality;
    }
}