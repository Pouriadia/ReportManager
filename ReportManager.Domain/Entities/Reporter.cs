using System;
using System.Text.RegularExpressions;
using ReportManager.Domain.Interfaces;

namespace ReportManager.Domain.Entities
{
    public class Reporter : IEntity<Guid>
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public DateTime HireDate { get; private set; }
        public string Bio { get; private set; }

        private Reporter() { }

        /// <summary>
        /// Factory method to create a new Reporter.
        /// Ensures invariants: valid email format, non-empty names.
        /// </summary>
        public static Reporter Create(
            string firstName,
            string lastName,
            string email,
            string phone,
            DateTime hireDate,
            string bio)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));

            // A simple but effective regex to ensure: local@domain.tld
            const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, emailPattern, RegexOptions.Compiled))
                throw new ArgumentException("Valid email is required.", nameof(email));

            return new Reporter
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                HireDate = hireDate,
                Bio = bio
            };
        }
    }
}