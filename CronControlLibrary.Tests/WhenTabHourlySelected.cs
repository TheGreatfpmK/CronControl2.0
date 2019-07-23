using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace CronControlLibrary.Tests
{
    [TestClass]
    public class WhenTabHourlySelected
    {
        // Arrange
        // Act
        // Assert

        private Fixture _fixture;
        private Func<decimal> _generator;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            var generator = _fixture.Create<Generator<decimal>>();
            _generator = () => { return generator.First(x => x > 1 && x <= 23); };
        }

        [TestMethod]
        public void ShouldBeInputedValue()
        {
            // Arrange
            var hours = _generator();
            var time = _fixture.Create<DateTime>();
            var control = new CronControl();
            control.SelectBoxIndex = 1;
            control.nudHourlyHours.Value = hours;
            control.dtpHourlyTime.Value = time;

            // Act
            var result = control.GetHourlyExpression();

            // Assert
            result.Should().Be($"0 {time.Minute} {time.Hour}/{hours} 1/1 * ? *");
        }

        [TestMethod]
        public void ShouldThrowOnZero()
        {
            // Arrange
            var control = new CronControl();
            control.SelectBoxIndex = 1;

            // Act
            Action result = () => control.nudHourlyHours.Value = 0;

            // Assert
            result.ShouldThrowExactly<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ShouldThrowOnGreaterThenMax()
        {
            // Arrange
            var control = new CronControl();
            control.SelectBoxIndex = 1;

            // Act
            Action result = () => control.nudHourlyHours.Value = 23 + _generator();

            // Assert
            result.ShouldThrowExactly<ArgumentOutOfRangeException>();
        }
    }
}
