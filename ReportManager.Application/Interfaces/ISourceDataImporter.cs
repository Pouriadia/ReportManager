namespace ReportManager.Application.Interfaces
{
    public interface ISourceDataImporter
    {
        /// <summary>
        /// Reads all CSV and .db files from the given directory and imports them.
        /// </summary>
        /// <param name="folderPath">Absolute path to the folder containing CSV/DB inputs.</param>
        Task ImportAllAsync(string folderPath);
    }
}