using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.News;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Domain.Tests.News
{
    [TestClass]
    public class NewsTests
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void CreateSuccess()
        {
            var newsEntry = NewsEntry.Create();
            newsEntry.AddContent(LanguageCodes.English, "title", "text");
            newsEntry.AddContent(LanguageCodes.German, "titel", "deutscher text");

            // Assert
            Assert.AreEqual("title", newsEntry.GetContentForLanguage(LanguageCodes.English).Title);
            Assert.AreEqual("titel", newsEntry.GetContentForLanguage(LanguageCodes.German).Title);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AddDuplicateContentForLanguage()
        {
            // Arrange

            // Act
            var newsEntry = NewsEntry.Create();
            newsEntry.AddContent(LanguageCodes.English, "title", "text");
            newsEntry.AddContent(LanguageCodes.English, "title2", "text2");

            // Assert
        }
    }

    public static class LanguageCodes
    {
        public const string English = "en";

        public const string German = "de";
    }
}
