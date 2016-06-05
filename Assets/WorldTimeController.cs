using UnityEngine;
using System.Collections;
using System;

public class WorldTimeController : MonoBehaviour
{

    public Weekday weekday;
    public int day;
    public int hour;
    public string toString = "lolnotset";

    private static WorldTimeController worldTimeController;
    public static WorldTimeController Instance()
    {
        if (!worldTimeController)
        {
            worldTimeController = FindObjectOfType(typeof(WorldTimeController)) as WorldTimeController;
            if (!worldTimeController)
                Debug.LogError("There needs to be one active WorldTimeController script on a GameObject in your scene.");
        }

        return worldTimeController;
    }

    private void Start()
    {
        InvokeRepeating("UpdateTimeData", 0, Settings.World_BaseTimeSecondsPerDay / 24.0f);
    }

    private void UpdateTimeData()
    {
        float elapsedGameDays = (Time.time / Settings.World_BaseTimeSecondsPerDay);
        TimeSpan gameTimeSpan = TimeSpan.FromDays(elapsedGameDays);

        day = gameTimeSpan.Days;
        hour = gameTimeSpan.Hours;

        string weekdayString = GetWeekdayString(day);
        string prettyHour = (gameTimeSpan.Hours < 10 ? "0" + gameTimeSpan.Hours : gameTimeSpan.Hours.ToString()) + ":00";
        toString = string.Format("Day {0}, {2} {1}", day, weekdayString, prettyHour);
    }

    private string GetWeekdayString(int day)
    {
        int dayModulo = day % 7;
        Weekday weekday = (Weekday)dayModulo;

        if (weekday == Weekday.Monday)
            return "Mon";
        if (weekday == Weekday.Tuesday)
            return "Tue";
        if (weekday == Weekday.Wednesday)
            return "Wed";
        if (weekday == Weekday.Thursday)
            return "Thu";
        if (weekday == Weekday.Friday)
            return "Fri";
        if (weekday == Weekday.Saturday)
            return "Sat";
        if (weekday == Weekday.Sunday)
            return "Sun";

        throw new Exception("Invalid day modulo: " + dayModulo + ". What the hell is going on, should not reach this code.");
    }

}

public enum Weekday
{
    Monday = 0,
    Tuesday = 1,
    Wednesday = 2,
    Thursday = 3,
    Friday = 4,
    Saturday = 5,
    Sunday = 6
}