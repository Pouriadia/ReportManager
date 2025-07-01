using System;
using FluentAssertions;
using ReportManager.Domain.Entities;
using Xunit;

namespace ReportManager.Tests.UnitTests
{
    public class DomainEntitiesTests
    {
        [Theory]
        [InlineData(null, "content", "summary", 3, "country")]
        [InlineData("", "content", "summary", 3, "country")]
        [InlineData("title", null, "summary", 3, "country")]
        [InlineData("title", "", "summary", 3, "country")]
        [InlineData("title", "content", null, 3, "country")]
        [InlineData("title", "content", "", 3, "country")]
        public void Article_Create_InvalidTextParams_ThrowsArgumentException(
            string title, string content, string summary, int importance, string country)
        {
            // Arrange & Act
            Action act = () =>
                Article.Create(
                    title,
                    content,
                    summary,
                    DateTime.UtcNow,
                    importance,
                    country,
                    Guid.NewGuid());

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public void Article_Create_InvalidImportance_ThrowsArgumentOutOfRangeException(int importance)
        {
            Action act = () =>
                Article.Create(
                    "title",
                    "content",
                    "summary",
                    DateTime.UtcNow,
                    importance,
                    "country",
                    Guid.NewGuid());

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(null, "Last", "email@example.com")]
        [InlineData("", "Last", "email@example.com")]
        [InlineData("First", null, "email@example.com")]
        [InlineData("First", "", "email@example.com")]
        public void Reporter_Create_InvalidNames_ThrowsArgumentException(
            string firstName, string lastName, string email)
        {
            Action act = () =>
                Reporter.Create(
                    firstName,
                    lastName,
                    email,
                    "phone",
                    DateTime.UtcNow,
                    "bio");

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("invalidemail")]
        [InlineData("invalid@.com")]
        [InlineData("invalid@com")]
        public void Reporter_Create_InvalidEmail_ThrowsArgumentException(string email)
        {
            Action act = () =>
                Reporter.Create(
                    "First",
                    "Last",
                    email,
                    "phone",
                    DateTime.UtcNow,
                    "bio");

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage("Valid email is required.*");
        }
    }
}
