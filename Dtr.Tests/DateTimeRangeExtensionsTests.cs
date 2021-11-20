using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dtr.Tests
{
    public class DateTimeRangeExtensionsTests
    {
        [Theory]
        [InlineData("2020-01-01 08:00:00&2020-01-10 08:00:00")]
        [InlineData("2020-01-01T08:00:00&2020-01-10T08:00:00")]
        [InlineData("2020-01-01T15:00:00+07:00&2020-01-10T15:00:00+07:00")]
        public void DateTimeRangeExtensions_ToDateTimeRangeList_IsSingleDayRange_SuccessCases(string stringDate)
        {
            //Arrange

            var expectResult = new List<DateTimeRange>
            {
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 01, 08, 00, 00),
                    ToDate = new DateTime(2020, 01, 10, 08, 00, 00)
                }
            };

            //Act
            var result = stringDate.ToDateTimeRangeList().ToList();
            
            // Assert
            Assert.True(expectResult.Count == result.Count);
            for (var i = 0; i < expectResult.Count; i++)
            {
                Assert.True(expectResult[i].FromDate.Equals(result[i].FromDate)
                            && expectResult[i].ToDate.Equals(result[i].ToDate));
            }
        }

        [Theory]
        [InlineData("2020-01-01 08:00:00&2020-01-02 08:00:00|2020-01-04 08:00:00&2020-01-05 08:00:00|2020-01-08 08:00:00&2020-01-09 08:00:00")]
        [InlineData("2020-01-01T08:00:00&2020-01-02T08:00:00|2020-01-04T08:00:00&2020-01-05T08:00:00|2020-01-08T08:00:00&2020-01-09T08:00:00")]
        [InlineData("2020-01-01T15:00:00+07:00&2020-01-02T15:00:00+07:00|2020-01-04T15:00:00+07:00&2020-01-05T15:00:00+07:00|2020-01-08T15:00:00+07:00&2020-01-09T15:00:00+07:00")]
        public void DateTimeRangeExtensions_ToDateTimeRangeList_HasMultipleDayRange_SuccessCases(string stringDate)
        {
            //Arrange

            var expectResult = new List<DateTimeRange>
            {
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 01, 08, 00, 00),
                    ToDate = new DateTime(2020, 01, 02, 08, 00, 00)
                },
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 04, 08, 00, 00),
                    ToDate = new DateTime(2020, 01, 05, 08, 00, 00)
                },
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 08, 08, 00, 00),
                    ToDate = new DateTime(2020, 01, 09, 08, 00, 00)
                }
            };

            //Act
            var result = stringDate.ToDateTimeRangeList().ToList();
            
            // Assert
            Assert.True(expectResult.Count == result.Count);
            for (var i = 0; i < expectResult.Count; i++)
            {
                Assert.True(expectResult[i].FromDate.Equals(result[i].FromDate)
                            && expectResult[i].ToDate.Equals(result[i].ToDate));
            }
        }

        [Theory]
        [InlineData("2020-02-30")]
        [InlineData("2020-01-01&2020-30-12")]
        [InlineData("2020-01-01&2020-01-02|2020-01-04&2020-01-05|2020-01-08&2020-02-30")]
        public void DateTimeRangeExtensions_ToDateTimeRangeList_ExpectEmpty(string stringDate)
        {
            //Act & Assert
            Assert.Empty(stringDate.ToDateTimeRangeList());
        }

        [Theory]
        [InlineData(
            "2020-01-07 08:00:00&2020-01-08 08:00:00|2020-01-04 08:00:00&2020-01-05 08:00:00|2020-01-01 08:00:00&2020-01-02 08:00:00")]
        [InlineData(
            "2020-01-07 08:00:00&2020-01-08 08:00:00|2020-01-01 08:00:00&2020-01-02 08:00:00|2020-01-04 08:00:00&2020-01-05 08:00:00")]
        [InlineData(
            "2020-01-04 08:00:00&2020-01-05 08:00:00|2020-01-07 08:00:00&2020-01-08 08:00:00|2020-01-01 08:00:00&2020-01-02 08:00:00")]
        [InlineData(
            "2020-01-07T08:00:00&2020-01-08T08:00:00|2020-01-04T08:00:00&2020-01-05T08:00:00|2020-01-01T08:00:00&2020-01-02T08:00:00")]
        [InlineData(
            "2020-01-07T08:00:00&2020-01-08T08:00:00|2020-01-01T08:00:00&2020-01-02T08:00:00|2020-01-04T08:00:00&2020-01-05T08:00:00")]
        [InlineData(
            "2020-01-04T08:00:00&2020-01-05T08:00:00|2020-01-07T08:00:00&2020-01-08T08:00:00|2020-01-01T08:00:00&2020-01-02T08:00:00")]
        public void DateTimeRangeExtensions_ToDateTimeRangeList_IsMultipleDateRange_Sorting_ExpectSuccess(
            string stringData)
        {
            //Arrange
            var expectResult = new List<DateTimeRange>
            {
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 01, 08, 00, 00),
                    ToDate = new DateTime(2020, 01, 02, 08, 00, 00)
                },
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 04, 08, 00, 00),
                    ToDate = new DateTime(2020, 01, 05, 08, 00, 00)
                },
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 07, 08, 00, 00),
                    ToDate = new DateTime(2020, 01, 08, 08, 00, 00)
                }
            };

            //Act
            var result = stringData.ToDateTimeRangeList();

            //Assert
            var dateRanges = result.ToList();
            Assert.True(expectResult.Count == dateRanges.Count);
            for (var i = 0; i < expectResult.Count; i++)
            {
                Assert.True(expectResult[i].FromDate.Equals(dateRanges[i].FromDate)
                            && expectResult[i].ToDate.Equals(dateRanges[i].ToDate));
            }
        }

        [Theory]
        [InlineData("2020-01-01&2020-01-04|2020-01-03&2020-01-06|2020-01-05&2020-01-09")]
        [InlineData("2020-01-04&2020-01-09|2020-01-03&2020-01-04|2020-01-01&2020-01-07")]
        [InlineData("2020-01-01&2020-01-09|2020-01-03&2020-01-04|2020-01-07&2020-01-09")]
        [InlineData("2020-01-01&2020-01-05|2020-01-04&2020-01-07|2020-01-04&2020-01-09")]
        [InlineData(
            "2020-01-01&2020-01-02|2020-01-02&2020-01-03|2020-01-03&2020-01-04|2020-01-04&2020-01-07|2020-01-07&2020-01-09")]
        public void
            DateTimeRangeExtensions_ToDateTimeRangeList_IsMultipleDateRange_OverlapsDateRange_ExpectMergedDateRanges(
                string stringData)
        {
            //Arrange
            var expectResult = new List<DateTimeRange>
            {
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 01),
                    ToDate = new DateTime(2020, 01, 09)
                }
            };

            //Act
            var result = stringData.ToDateTimeRangeList();

            //Assert
            var dateRanges = result.ToList();
            Assert.True(expectResult.Count == dateRanges.Count);
            for (var i = 0; i < expectResult.Count; i++)
            {
                Assert.True(expectResult[i].FromDate.Equals(dateRanges[i].FromDate)
                            && expectResult[i].ToDate.Equals(dateRanges[i].ToDate));
            }
        }

        [Theory]
        [InlineData("2020-01-01&2020-01-05|2020-01-04&2020-01-07|2020-01-08&2020-01-09")]
        public void
            DateTimeRangeExtensions_ToDateTimeRangeList_IsMultipleDateRange_OverlapsAndContinuityDateRange_ExpectNotMergedDateRanges(string stringDate)
        {
            //Arrange
            var expectResult = new List<DateTimeRange>
            {
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 01),
                    ToDate = new DateTime(2020, 01, 07)
                },
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 08),
                    ToDate = new DateTime(2020, 01, 09)
                }
            };

            //Act 
            var result = stringDate.ToDateTimeRangeList();

            //Assert
            var dateRanges = result.ToList();
            Assert.True(expectResult.Count == dateRanges.Count);
            for (var i = 0; i < expectResult.Count; i++)
            {
                Assert.True(expectResult[i].FromDate.Equals(dateRanges[i].FromDate) &&
                            expectResult[i].ToDate.Equals(dateRanges[i].ToDate));
            }
        }

        [Theory]
        [InlineData("2020-01-01 08:00:00")]
        [InlineData("2020-01-01T08:00:00")]
        [InlineData("2020-01-01T01:00:00-07:00")]
        public void DateTimeRangeExtensions_ToDateTimeRangeList_IsSingleDate_ExpectSuccess(string stringDate)
        {
            // Arrange
            var expectResult = new List<DateTimeRange>
            {
                new DateTimeRange
                {
                    FromDate = new DateTime(2020, 01, 01, 08, 00, 00)
                }
            };

            // Act
            var result = stringDate.ToDateTimeRangeList();

            // Assert
            var dateRanges = result.ToList();
            Assert.True(expectResult.Count == dateRanges.Count);
            for (var i = 0; i < expectResult.Count; i++)
            {
                Assert.True(expectResult[i].FromDate.Equals(dateRanges[i].FromDate) &&
                            expectResult[i].ToDate.Equals(dateRanges[i].ToDate));
            }
        }
    }
}