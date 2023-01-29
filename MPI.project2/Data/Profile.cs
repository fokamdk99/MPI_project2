namespace MPI.project2.Data;

public class Profile
{
    public int ContentId { get; set; }
    public int ProfileId { get; set; }
    public float Size { get; set; } // unit: GB
    public int ProfileQuality { get; set; }
    public float StorageCost { get; set; }
    public float TranscodingCost { get; set; }
    public float AnnealingCoefficient { get; set; }

    public Profile(int profileId,
        int contentId, 
        float size, 
        int profileQuality)
    {
        ProfileId = profileId;
        ContentId = contentId;
        Size = size;
        ProfileQuality = profileQuality;
    }
}