using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace CronControlLibrary.Tests
{
    [TestClass]
    public class WhenTabDailySelected
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
            _generator = () => { return generator.First(x => x > 1 && x <= 360); };
        }

        [TestMethod]
        public void ShouldChangeSelectedRadioToWeekdays()
        {
            // Arrange
            var control = new CronControl();
            control.SelectBoxIndex = 2;

            // Act
            control.rbtDailyWeekDays.Checked = true;

            // Assert
            control.rbtDailyWeekDays.Checked.Should().BeTrue();
            control.rbtDailyEvery.Checked.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldChangeSelectedRadioToEvery()
        {
            // Arrange
            var control = new CronControl();
            control.SelectBoxIndex = 2;

            // Act
            control.rbtDailyEvery.Checked = true;

            // Assert
            control.rbtDailyEvery.Checked.Should().BeTrue();
            control.rbtDailyWeekDays.Checked.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldBeInputedValueWhenEverySelected()
        {
            // Arrange
            var days = _generator();
            var time = _fixture.Create<DateTime>();
            var control = new CronControl();
            control.SelectBoxIndex = 2;
            control.rbtDailyEvery.Checked = true;
            control.nudDailyDays.Value = days;
            control.dtpDailyTime.Value = time;

            // Act
            var result = control.GetDailyExpression();

            // Assert
            result.Should().Be($"0 {time.Minute} {time.Hour} 1/{days} * ? *");
        }

        [TestMethod]
        public void ShouldBeInputedValueWhenWeekdaysSelected()
        {
            // Arrange
            var time = _fixture.Create<DateTime>();
            var control = new CronControl();
            control.SelectBoxIndex = 2;
            control.rbtDailyWeekDays.Checked = true;
            control.dtpDailyTime.Value = time;

            // Act
            var result = control.GetDailyExpression();

            // Assert
            result.Should().Be($"0 {time.Minute} {time.Hour} ? * 2-6 *");
        }
    }
}
