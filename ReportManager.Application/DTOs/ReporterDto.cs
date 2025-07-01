using System;

namespace ReportManager.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Reporter entity.
    /// Used to carry reporter data between layers and to the API.
    /// </summary>
    public class ReporterDto
    {
        /// <summary>
        /// Unique identifier of the reporter.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First name of the reporter.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the reporter.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Reporter’s email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Reporter’s phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Date when the reporter was hired.
        /// </summary>
        public DateTime HireDate { get; set; }
    }
}