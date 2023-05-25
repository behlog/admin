using System.Globalization;

namespace Behlog.Web.Admin.Models;

public static class PersianDateCalculator
{

    /// <summary>
    /// Get DateTime from persian date in string format like : 1402/02/30 22:20:00
    /// </summary>
    /// <param name="date">The Date part</param>
    /// <param name="time">The Time part</param>
    /// <returns>DateTime in according to the string values.</returns>
    public static DateTime? GetFromString(this string? date, string? time) {
        if (string.IsNullOrWhiteSpace(date))
            return null;

        //1402/02/30
        var persian = new PersianCalendar();
        int year = int.Parse(date.Substring(0, 4));
        int month = int.Parse(date.Substring(5, 2));
        int day = int.Parse(date.Substring(8, 2));

        if (string.IsNullOrEmpty(time))
            return persian.ToDateTime(year, month, day, 0, 0, 0, 0);

        //21:22:00
        int hour = int.Parse(time.Substring(0, 2));
        int min = int.Parse(time.Substring(3, 2));
        int sec = int.Parse(time.Substring(6, 2));

        return persian.ToDateTime(year, month, day, hour, min, sec, 0);
    }

    public static string? GetPersianDate(this DateTime? dateTime)
    {
        if (dateTime is null) return null;
        
        var persian = new PersianCalendar();
        var localDate = dateTime.Value.ToLocalTime();
        int year = persian.GetYear(localDate);
        int month = persian.GetMonth(localDate);
        int day = persian.GetDayOfMonth(localDate);

        return $"{year}/{month}/{day}";
    }

    public static string? GetPersianTime(this DateTime? dateTime)
    {
        if (dateTime is null) return null;
        
        int hour = dateTime.Value.Hour;
        int min = dateTime.Value.Minute;
        int sec = dateTime.Value.Second;

        return $"{hour}:{min}:{sec}";
    }
}