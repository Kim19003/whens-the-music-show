using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whens_the_music_show
{
    internal class ProgramData
    {
        public string Creator { get; }
        public double Version { get; }

        public ProgramData(string creator, double version)
        {
            Creator = creator;
            Version = version;
        }
    }

    internal class MusicShow
    {
        public string Name { get; set; }
        public string Organizer { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public MusicShow(string name, string organizer, DateTime startTime, DateTime endTime)
        {
            Name = name;
            Organizer = organizer;
            StartTime = startTime;
            EndTime = endTime;
        }
    }

    internal class Time
    {
        public int Hour { get; set; }
        public int Minute { get; set; }

        private int dayDifference = 0;

        public Time(int hour, int minute)
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

        public void AddHours(int hours)
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

        public void RemoveHours(int hours)
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

        public void AddMinutes(int minutes)
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

        public void RemoveMinutes(int minutes)
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

        public int GetDayDifference()
        {
            return dayDifference;
        }
    }
}
