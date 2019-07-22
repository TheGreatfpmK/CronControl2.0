using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace CronControlLibrary.Tests
{
    [TestClass]
    public class WhenGetValue
    {
        // Arrange
        // Act
        // Assert

        private Fixture _fixture;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public void ShouldGetMinutesFormatIfMinutesTabSelected()
        {
            // Arrange
            var control = new CronControl();
            control.tcMain.SelectTab("tabMinutes");

            // Act
            var result = control.Value;

            // Assert
            Regex.IsMatch(result, @"0 0/\d+ \* 1/1 \* \? \*", RegexOptions.IgnoreCase).Should().BeTrue();
        }

        [TestMethod]
        public void ShouldGetHourlyFormatIfHoursTabSelected()
        {
            // Arrange
            var control = new CronControl();
            control.tcMain.SelectTab("tabHourly");

            // Act
            var result = control.Value;

            // Assert
            Regex.IsMatch(result, @"0 \d+ \d+/\d+ 1/1 \* \? \*", RegexOptions.IgnoreCase).Should().BeTrue();
        }

        [TestMethod]
        public void ShouldGetDailyFormatIfDaysTabSelected()
        {
            // Arrange
            var control = new CronControl();
            control.tcMain.SelectTab("tabDaily");

            // Act
            var result = control.Value;

            // Assert
            Regex.IsMatch(result, @"0 \d+ \d+ 1/\d+ \* \? \*", RegexOptions.IgnoreCase).Should().BeTrue();
        }

        [TestMethod]
        public void ShouldGetDailyFormatIfDaysTabAndWeekDaysSelected()
        {
            // Arrange
            var control = new CronControl();
            control.tcMain.SelectTab("tabDaily");
            control.rbtDailyWeekDays.Checked = true;

            // Act
            var result = control.Value;

            // Assert
            Regex.IsMatch(result, @"0 \d+ \d+ \? \* 2-6 \*", RegexOptions.IgnoreCase).Should().BeTrue();
        }

        [TestMethod]
        public void ShouldGetWeeklyFormatIfWeeksTabSelected()
        {
            // Arrange
            var checks = _fixture.CreateMany<bool>(7).ToList();

            var control = new CronControl();
            control.tcMain.SelectTab("tabWeekly");
            control.cbxSunday.Checked = checks[0];
            control.cbxMonday.Checked = checks[1];
            control.cbxTuesday.Checked = checks[2];
            control.cbxWednesday.Checked = checks[3];
            control.cbxThursday.Checked = checks[4];
            control.cbxFriday.Checked = checks[5];
            control.cbxSaturday.Checked = checks[6];

            // Act
            var result = control.Value;

            // Assert
            Regex.IsMatch(result, @"0 \d+ \d+ \? \* (\d,?)+ \*", RegexOptions.IgnoreCase).Should().BeTrue();
        }

        [TestMethod]
        public void ShouldGetMonthlyFormatIfMonthsTabSelected()
        {
            // Arrange
            var control = new CronControl();
            control.tcMain.SelectTab("tabMonthly");

            // Act
            var result = control.Value;

            // Assert
            Regex.IsMatch(result, @"0 \d+ \d+ \d+ 1/\d+ \? \*", RegexOptions.IgnoreCase).Should().BeTrue();
        }

        [TestMethod]
        public void ShouldGetMonthlyFormatIfMonthsTabAndOrdinalSelected()
        {
            // Arrange
            var control = new CronControl();
            control.tcMain.SelectTab("tabMonthly");
            control.rbtMonthlyOrdinal.Checked = true;

            // Act
            var result = control.Value;

            // Assert
            Regex.IsMatch(result, @"0 \d+ \d+ \? 1/\d+ \d(#\d|L) \*", RegexOptions.IgnoreCase).Should().BeTrue();
        }

        [TestMethod]
        public void ShouldGetYearlyFormatIfYearsTabSelected()
        {
            // Arrange
            var control = new CronControl();
            control.tcMain.SelectTab("tabYearly");

            // Act
            var result = control.Value;

            // Assert
            Regex.IsMatch(result, @"0 \d+ \d+ \d+ \d+ \? \*", RegexOptions.IgnoreCase).Should().BeTrue();
        }

        [TestMethod]
        public void ShouldGetYearlyFormatIfYearsTabAndOrdinalSelected()
        {
            // Arrange
            var control = new CronControl();
            control.tcMain.SelectTab("tabYearly");
            control.rbtYearlyOrdinal.Checked = true;

            // Act
            var result = control.Value;

            // Assert
            Regex.IsMatch(result, @"0 \d+ \d+ \? \d+ \d(#\d|L) \*", RegexOptions.IgnoreCase).Should().BeTrue();
        }
    }
}
