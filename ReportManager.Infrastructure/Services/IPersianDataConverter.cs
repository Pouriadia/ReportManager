using System;

namespace ReportManager.Infrastructure.Services
{
    /// <summary>
    /// Defines conversion from Persian (Shamsi) date strings to DateTime.
    /// </summary>
    public interface IPersianDateConverter
    {
        /// <summary>
        /// Converts a Persian date string (e.g., "1404/03/24") to a UTC DateTime.
        /// </summary>
        /// <param name="persianDate">Date string in yyyy/MM/dd format.</param>
        /// <returns>DateTime in UTC.</returns>
        DateTime ConvertToDateTime(string persianDate);
    }
}