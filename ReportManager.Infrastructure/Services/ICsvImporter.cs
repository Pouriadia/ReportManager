namespace ReportManager.Infrastructure.Services
{
    /// <summary>
    /// Defines contract for importing Article data from CSV streams.
    /// </summary>
    public interface ICsvImporter
    {
        /// <summary>
        /// Imports articles from the given CSV file stream.
        /// </summary>
        /// <param name="csvStream">Stream containing CSV data.</param>
        void ImportArticles(Stream csvStream);
    }
}