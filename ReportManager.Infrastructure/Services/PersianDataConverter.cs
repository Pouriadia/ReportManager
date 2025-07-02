using System.Globalization;

namespace ReportManager.Infrastructure.Services
{
    public class PersianDataConverter : IPersianDateConverter
    {
        /// <summary>
        /// Converts a date string which can be either:
        ///  - Persian date in "yyyy/MM/dd"
        ///  - Gregorian ISO date in "yyyy-MM-dd"
        /// to a UTC DateTime.
        /// </summary>
        public DateTime ConvertToDateTime(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                throw new ArgumentException("Date string is required.", nameof(dateString));

            dateString = dateString.Trim();

            // If the date contains '-', assume ISO format yyyy-MM-dd
            if (dateString.Contains('-'))
            {
                if (DateTime.TryParseExact(
                        dateString,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                        out var dtIso))
                {
                    return dtIso;
                }
                else
                {
                    throw new FormatException($"Invalid ISO date format. Expected yyyy-MM-dd. Got '{dateString}'.");
                }
            }
            // Otherwise assume Persian date yyyy/MM/dd
            else if (dateString.Contains('/'))
            {
                var parts = dateString.Split('/');
                if (parts.Length != 3
                    || !int.TryParse(parts[0], out var py)
                    || !int.TryParse(parts[1], out var pm)
                    || !int.TryParse(parts[2], out var pd))
                {
                    throw new FormatException($"Invalid Persian date format. Expected yyyy/MM/dd. Got '{dateString}'.");
                }
                var persian = new PersianCalendar();
                var dt = persian.ToDateTime(py, pm, pd, 0, 0, 0, 0);
                return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }
            else
            {
                throw new FormatException($"Unrecognized date format: '{dateString}'.");
            }
        }
    }
}
