using NUnit.Framework;

namespace Bds.TechTest.Tests
{
    public class GoogleEngineTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCreation()
        {
            // Just check the properties are all as we expect

            Assert.Fail("Test not written yet");
        }

        [Test]
        public void TestCorrectQuery()
        {
            // Test by passing in a Moq of the HTTP runner. Should the runner be threaded? Probably - use async...await

            // Use the Moq to check that it is forming the query the way we expect

            Assert.Fail("Test not written yet");
        }

        [Test]
        public void TestResultParsing()
        {
            // Test by passing in a Moq that returns a page that the engine parses and check that the results come back as we expect

            Assert.Fail("Test not written yet");
        }
    }
}