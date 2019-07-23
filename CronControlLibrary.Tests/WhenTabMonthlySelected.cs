﻿using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace CronControlLibrary.Tests
{
    [TestClass]
    public class WhenTabMonthlySelected
    {
        // Arrange
        // Act
        // Assert

        private Fixture _fixture;
        private Func<int, int, int> _intGenerator;
        private Func<int, int, decimal> _decimalGenerator;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            var decimalGenerator = _fixture.Create<Generator<decimal>>();
            _decimalGenerator = (begin, end) => { return decimalGenerator.First(x => x > begin && x <= end); };

            var intGenerator = _fixture.Create<Generator<int>>();
            _intGenerator = (begin, end) => { return intGenerator.First(x => x > begin && x <= end); };
        }

        [TestMethod]
        public void ShouldChangeSelectedRadioToEvery()
        {
            // Arrange
            var control = new CronControl();
            control.SelectBoxIndex = 4;

            // Act
            control.rbtMonthlyEvery.Checked = true;

            // Assert
            control.rbtMonthlyEvery.Checked.Should().BeTrue();
            control.rbtMonthlyOrdinal.Checked.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldChangeSelectedRadioToOrdinal()
        {
            // Arrange
            var control = new CronControl();
            control.SelectBoxIndex = 4;

            // Act
            control.rbtMonthlyOrdinal.Checked = true;

            // Assert
            control.rbtMonthlyOrdinal.Checked.Should().BeTrue();
            control.rbtMonthlyEvery.Checked.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldBeInputedValueWhenEverySelected()
        {
            // Arrange
            var days = _decimalGenerator(1, 31);
            var months = _decimalGenerator(1, 12);
            var time = _fixture.Create<DateTime>();
            var control = new CronControl();
            control.SelectBoxIndex = 4;
            control.rbtMonthlyEvery.Checked = true;
            control.dtpMonthlyTime.Value = time;
            control.nudMonthlyDays.Value = days;
            control.nudMonthlyMonths.Value = months;

            // Act
            var result = control.GetMonthlyExpression();

            // Assert
            result.Should().Be($"0 {time.Minute} {time.Hour} {days} 1/{months} ? *");
        }

        [TestMethod]
        public void ShouldBeInputedValueWhenOrdinalSelected()
        {
            // Arrange
            var ordinal = _intGenerator(0, 4);
            var weekday = _intGenerator(0, 6);
            var months = _decimalGenerator(1, 12);
            var time = _fixture.Create<DateTime>();
            var control = new CronControl();
            control.SelectBoxIndex = 4;
            control.rbtMonthlyOrdinal.Checked = true;
            control.dtpMonthlyTime.Value = time;
            control.nudMonthlyMonths2.Value = months;
            control.ddlMonthlyOrdinal.SelectedIndex = ordinal;
            control.ddlMonthlyWeekdays.SelectedIndex = weekday;

            var ordinalText = ordinal < 4 ? $"#{ordinal + 1}" : "L";

            // Act
            var result = control.GetMonthlyExpression();

            // Assert
            result.Should().Be($"0 {time.Minute} {time.Hour} ? 1/{months} {weekday + 1}{ordinalText} *");
        }

        [TestMethod]
        public void ShouldThrowOnZero()
        {
            // Arrange
            var control = new CronControl();
            control.SelectBoxIndex = 4;

            // Act
            Action result1 = () => control.nudMonthlyMonths.Value = 0;
            Action result2 = () => control.nudMonthlyMonths2.Value = 0;
            Action result3 = () => control.nudMonthlyDays.Value = 0;

            // Assert
            result1.ShouldThrowExactly<ArgumentOutOfRangeException>();
            result2.ShouldThrowExactly<ArgumentOutOfRangeException>();
            result3.ShouldThrowExactly<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ShouldThrowOnGreaterThenMax()
        {
            // Arrange
            var control = new CronControl();
            control.SelectBoxIndex = 4;

            // Act
            Action result1 = () => control.nudMonthlyMonths.Value = _decimalGenerator(13, 100);
            Action result2 = () => control.nudMonthlyMonths2.Value = _decimalGenerator(13, 100);
            Action result3 = () => control.nudMonthlyDays.Value = _decimalGenerator(32, 100);

            // Assert
            result1.ShouldThrowExactly<ArgumentOutOfRangeException>();
            result2.ShouldThrowExactly<ArgumentOutOfRangeException>();
            result3.ShouldThrowExactly<ArgumentOutOfRangeException>();
        }
    }
}
