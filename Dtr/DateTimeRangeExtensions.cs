using System;
using System.Collections.Generic;
using System.Linq;

namespace Dtr
{
    public static class DateTimeRangeExtensions
    {
        public static IEnumerable<DateTimeRange> ToDateTimeRangeList(this string dateString)
        {
            try
            {
                if (string.IsNullOrEmpty(dateString)) return Enumerable.Empty<DateTimeRange>();
                if (IsMultipleDateRange(dateString, out var dateRangeMultiple))
                {
                    return ToMultipleDateRange(dateRangeMultiple);
                }

                if (IsSingleDateRange(dateString, out var dateRangeSingle))
                {
                    return ToSingleDateRanges(dateRangeSingle);
                }

                if (IsSingleDate(dateString, out var dates))
                {
                    return ToSingleDate(dates);
                }

                return Enumerable.Repeat(new DateTimeRange
                {
                    FromDate = DateTime.Parse(dateString).ToUniversalTime(),
                    ToDate = DateTime.Parse(dateString).AddDays(1).ToUniversalTime()
                }, 1);
            }
            catch
            {
                return Enumerable.Empty<DateTimeRange>();
            }
        }

        private static bool IsSingleDate(string dateString, out string[] dates)
        {
            dates = dateString.Split(new[] { '&' });
            return dates.Length == 1;
        }

        private static bool IsSingleDateRange(string dateString, out string[] dates)
        {
            dates = dateString.Split(new[] { '&' });
            return dates.Length > 1;
        }

        private static bool IsMultipleDateRange(string dateString, out string[] dates)
        {
            dates = dateString.Split(new[] { '|' });
            return dates.Length > 1;
        }

        private static IEnumerable<DateTimeRange> ToMultipleDateRange(IEnumerable<string> dateString)
        {
            var listRanges = dateString.Select(date => date.Split(new[] { '&' }))
                .Select(fromToDates => new DateTimeRange
                {
                    FromDate = DateTime.Parse(fromToDates[0]).ToUniversalTime(),
                    ToDate = DateTime.Parse(fromToDates[1]).ToUniversalTime()
                }).OrderBy(fromToDates => fromToDates.FromDate).ToList();
            var results = new List<DateTimeRange>();
            results.AddRange(listRanges);
            var indexOfResult = 0;
            for (var i = 0; i < listRanges.Count; i++)
            {
                var x = i + 1;
                if (x == listRanges.Count) break;
                if (listRanges[x].FromDate > results[indexOfResult].ToDate &&
                    (listRanges[x].FromDate <= results[indexOfResult].ToDate ||
                     listRanges[x].FromDate.Subtract(results[indexOfResult].ToDate).Days >= 1))
                {
                    indexOfResult++;
                    continue;
                }

                if (listRanges[x].ToDate > results[indexOfResult].ToDate)
                    results[indexOfResult].ToDate = listRanges[x].ToDate;

                var indexRemove = listRanges.Count == results.Count ? x : indexOfResult + 1;
                results.RemoveAt(indexRemove);
            }

            return results;
        }

        private static IEnumerable<DateTimeRange> ToSingleDateRanges(IReadOnlyList<string> dateString)
        {
            return Enumerable.Repeat(new DateTimeRange
            {
                FromDate = DateTime.Parse(dateString[0]).ToUniversalTime(),
                ToDate = DateTime.Parse(dateString[1]).ToUniversalTime()
            }, 1);
        }

        private static IEnumerable<DateTimeRange> ToSingleDate(IReadOnlyList<string> dateString)
        {
            return Enumerable.Repeat(new DateTimeRange
            {
                FromDate = DateTime.Parse(dateString[0]).ToUniversalTime()
            }, 1);
        }
    }
}