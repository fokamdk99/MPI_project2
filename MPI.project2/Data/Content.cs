namespace MPI.project2.Data;

public class Content
{
    public int ContentId { get; set; }
    public float ContentLength { get; set; } // unit: h
    public float Popularity { get; set; } // probability

    public Content(int contentId, float contentLength, float popularity)
    {
        ContentId = contentId;
        ContentLength = contentLength;
        Popularity = popularity;
    }
}