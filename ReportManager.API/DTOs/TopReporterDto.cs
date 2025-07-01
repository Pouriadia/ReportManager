namespace ReportManager.API.DTOs
{
    public class TopReporterDto
    {
        // Reporter info
        public Guid   Id         { get; set; }
        public string FirstName  { get; set; } = default!;
        public string LastName   { get; set; } = default!;

        // Count of articles
        public int    ArticleCount { get; set; }
    }
}