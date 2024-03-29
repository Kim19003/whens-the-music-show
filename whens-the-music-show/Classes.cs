﻿using System.Runtime.InteropServices;

namespace whens_the_music_show
{
    internal class AppSettings
    {
        public string? DefaultBrowserPath { get; set; }
        public int? DetailsCheckIntervalInDays { get; set; }
    }

    internal class DllImport
    {
        [DllImport("User32")]
        internal static extern int SetForegroundWindow(IntPtr hwnd);
        [DllImportAttribute("User32.DLL")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }

    internal class ProgramData
    {
        internal string Creator { get; }
        internal double Version { get; }

        internal ProgramData(string creator, double version)
        {
            Creator = creator;
            Version = version;
        }
    }

    internal class MusicShow
    {
        public string? Name { get; set; }
        public string? Organizer { get; set; }
        public DayOfWeek AirDay { get; set; }
        public DateTime StartTime { get; set; } = new DateTime(1920, 1, 1, 0, 0, 0);
        public DateTime EndTime { get; set; } = new DateTime(1920, 1, 1, 0, 0, 0);
    }

    internal class MusicShowRaw
    {
        public string? Name { get; set; }
        public string? Organizer { get; set; }
        public DayOfWeek AirDay { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
    }

    internal class Performance
    {
        internal string? Artist { get; set; }
        internal string? Song { get; set; }
        internal string? Stage { get; set; }
        internal string? Link { get; set; }

        internal Performance(string? artist, string? song, string? link)
        {
            Artist = artist;
            Song = song;
            Link = link;
        }

        internal void AddToStage(string stageName)
        {
            Stage = stageName;
        }
    }

    internal class Time
    {
        internal int Hour { get; set; }
        internal int Minute { get; set; }

        private int dayDifference = 0;

        internal Time(int hour, int minute)
        {
            if (hour < 24 && hour > -1)
            {
                Hour = hour;
            }
            else
            {
                Hour = 0;
            }
            if (minute < 60 && minute > -1)
            {
                Minute = minute;
            }
            else
            {
                Minute = 0;
            }
        }

        internal void AddHours(int hours)
        {
            int addedHours = Hour + hours;

            if (addedHours > 23)
            {
                dayDifference++;
                Hour = addedHours - 24;
            }
            else
            {
                Hour = addedHours;
            }
        }

        internal void RemoveHours(int hours)
        {
            int removedHours = Hour - hours;

            if (removedHours < 0)
            {
                dayDifference--;
                Hour = 24 - Math.Abs(removedHours);
            }
            else
            {
                Hour = removedHours;
            }
        }

        internal void AddMinutes(int minutes)
        {
            int addedMinutes = Minute + minutes;

            if (addedMinutes > 59)
            {
                AddHours(1);
                Minute = addedMinutes - 60;
            }
            else
            {
                Minute = addedMinutes;
            }
        }

        internal void RemoveMinutes(int minutes)
        {
            int removedMinutes = Minute - minutes;

            if (removedMinutes < 0)
            {
                RemoveHours(1);
                Minute = 60 - Math.Abs(removedMinutes);
            }
            else
            {
                Minute = removedMinutes;
            }
        }

        internal int GetDayDifference()
        {
            return dayDifference;
        }
    }
}
