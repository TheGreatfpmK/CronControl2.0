using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace CronControlLibrary.Tests
{
    [TestClass]
    public class WhenNeverSelected
    {
        private Fixture _fixture;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public void EmptyStringExpected()
        {
            // Arrange
            var control = new CronControl();
            control.SelectBoxIndex = 0;

            // Assert
            control.Value.Should().Be(string.Empty);

        }
    }
}