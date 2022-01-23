using Newtonsoft.Json;
using whens_the_music_show;

int timeZoneDifference = -7;

MusicShow TheShow = new MusicShow(
        "The Show", "SBS",
        SimpleTime(DayOfWeek.Tuesday, ConvertToTimeZone(timeZoneDifference, new Time(18, 00))),
        SimpleTime(DayOfWeek.Tuesday, ConvertToTimeZone(timeZoneDifference, new Time(19, 30)))
    );
MusicShow ShowChampion = new MusicShow(
        "Show Champion", "MBC",
        SimpleTime(DayOfWeek.Wednesday, ConvertToTimeZone(timeZoneDifference, new Time(18, 00))),
        SimpleTime(DayOfWeek.Wednesday, ConvertToTimeZone(timeZoneDifference, new Time(19, 30)))
    );
MusicShow MCountdown = new MusicShow(
        "M Countdown", "Mnet",
        SimpleTime(DayOfWeek.Thursday, ConvertToTimeZone(timeZoneDifference, new Time(18, 00))),
        SimpleTime(DayOfWeek.Thursday, ConvertToTimeZone(timeZoneDifference, new Time(19, 30)))
    );
MusicShow MusicBank = new MusicShow(
        "Music Bank", "KBS",
        SimpleTime(DayOfWeek.Friday, ConvertToTimeZone(timeZoneDifference, new Time(17, 00))),
        SimpleTime(DayOfWeek.Friday, ConvertToTimeZone(timeZoneDifference, new Time(18, 30)))
    );
MusicShow ShowMusicCore = new MusicShow(
        "Show! Music Core", "MBC",
        SimpleTime(DayOfWeek.Saturday, ConvertToTimeZone(timeZoneDifference, new Time(15, 45))),
        SimpleTime(DayOfWeek.Saturday, ConvertToTimeZone(timeZoneDifference, new Time(17, 05)))
    );
MusicShow Inkigayo = new MusicShow(
        "Inkigayo", "SBS",
        SimpleTime(DayOfWeek.Sunday, ConvertToTimeZone(timeZoneDifference, new Time(15, 40))),
        SimpleTime(DayOfWeek.Sunday, ConvertToTimeZone(timeZoneDifference, new Time(16, 50)))
    );

MusicShow[] musicShows = { TheShow, ShowChampion, MCountdown, MusicBank, ShowMusicCore, Inkigayo };

while (true)
{
    Header();

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("(1) Show all music shows\n(2) Show the next music show\n(3) Show music show winners");
    ConsoleKey selection = Console.ReadKey().Key;

    switch (selection)
    {
        case ConsoleKey.D1:
            Console.Clear();
            Header();
            ShowAllMusicShows(musicShows);
            Console.ReadKey();
            break;
        case ConsoleKey.D2:
            Console.Clear();
            Header();
            ShowNextMusicShow(musicShows);
            Console.ReadKey();
            break;
        case ConsoleKey.D3:
            Console.Clear();
            Header();
            await TryGetWinner(musicShows);
            Console.ReadKey();
            break;
    }

    Console.Clear();
}

#region Methods
static void Header()
{
    DateTime now = DateTime.Now;

    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("-------------------------");
    Console.WriteLine("| When's the music show |");
    Console.WriteLine("-------------------------");

    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\nTime now: ");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"{now:g}\n");

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("\n-------------------------\n");
}

static void ShowAllMusicShows(MusicShow[] musicShows)
{
    DateTime now = DateTime.Now;

    for (int i = 0; i < musicShows.Length; i++)
    {
        if (musicShows[i].StartTime.DayOfWeek == now.DayOfWeek)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        Console.WriteLine($"{musicShows[i].StartTime.DayOfWeek}: {musicShows[i].Name} ({musicShows[i].StartTime:g})");
    }
}

static void ShowNextMusicShow(MusicShow[] musicShows)
{
    DateTime now = DateTime.Now;

    MusicShow? airingNow = null;
    MusicShow? nextShow = null;

    foreach (MusicShow musicShow in musicShows)
    {
        if (musicShow.StartTime < now && musicShow.EndTime > now) // Airing now
        {
            airingNow = musicShow;
            break;
        }
        else if (musicShow.StartTime > now) // Next show
        {
            nextShow = musicShow;
            break;
        }
    }

    if (airingNow != null)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"'{airingNow.Name}' is airing now! ({now - airingNow.StartTime} minutes in)");
    }
    else if (nextShow != null)
    {
        if (nextShow.StartTime.Day > now.Day + 1) // More than two days difference
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"'{nextShow.Name}' airs next {nextShow.StartTime.DayOfWeek} at {nextShow.StartTime:t}.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"'{nextShow.Name}' airs tomorrow ({nextShow.StartTime.DayOfWeek.ToString().ToLower()}) at {nextShow.StartTime:t}!");
        }
    }
}

static DateTime SimpleTime(DayOfWeek dayOfWeek, Time time)
{
    if (time.GetDayDifference() == 1) // Can't be more than 1 in real life
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, GetNextDay(dayOfWeek) + 1, time.Hour, time.Minute, 00);
    }
    else if (time.GetDayDifference() == -1) // Can't be less than -1 in real life
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, GetNextDay(dayOfWeek) - 1, time.Hour, time.Minute, 00);
    }
    else
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, GetNextDay(dayOfWeek), time.Hour, time.Minute, 00);
    }
}

static Time ConvertToTimeZone(int timeDifference, Time now)
{
    Time convertedTime = new(now.Hour, now.Minute);

    if (timeDifference > 0)
    {
        convertedTime.AddHours(timeDifference);
    }
    else if (timeDifference < 0)
    {
        convertedTime.RemoveHours(Math.Abs(timeDifference));
    }

    return convertedTime;
}

static int GetNextDay(DayOfWeek dayOfWeek)
{
    DateTime now = DateTime.Now;

    if (now.DayOfWeek != dayOfWeek)
    {
        DateTime result = DateTime.Now.AddDays(1);

        while (result.DayOfWeek != dayOfWeek)
        {
            result = result.AddDays(1);
        }

        return result.Day;
    }

    return now.Day;
}

static async Task TryGetWinner(MusicShow[] musicShows)
{
    DateTime now = DateTime.Now;

    for (int i = 0; i < musicShows.Length; i++)
    {
        Console.WriteLine($"({i+1}) {musicShows[i].Name}");
    }

    ConsoleKey selection = Console.ReadKey().Key;

    string? eventToShow = null;

    int select = 0;
    switch (selection)
    {
        case ConsoleKey.D1:
            select = 1;
            if (musicShows[0].StartTime.DayOfWeek != now.DayOfWeek)
            {
                DateTime weekAgo = musicShows[0].StartTime.AddDays(-7);
                eventToShow = $"the-show/{weekAgo:yyyy}{weekAgo:MM}{weekAgo:dd}";
            }
            else
            {
                eventToShow = $"the-show/{musicShows[0].StartTime:yyyy}{musicShows[0].StartTime:MM}{musicShows[0].StartTime:dd}";
            }
            break;
        case ConsoleKey.D2:
            select = 2;
            if (musicShows[1].StartTime.DayOfWeek != now.DayOfWeek)
            {
                DateTime weekAgo = musicShows[1].StartTime.AddDays(-7);
                eventToShow = $"show-champion/{weekAgo:yyyy}{weekAgo:MM}{weekAgo:dd}";
            }
            else
            {
                eventToShow = $"show-champion/{musicShows[1].StartTime:yyyy}{musicShows[1].StartTime:MM}{musicShows[1].StartTime:dd}";
            }
            break;
        case ConsoleKey.D3:
            select = 3;
            if (musicShows[2].StartTime.DayOfWeek != now.DayOfWeek)
            {
                DateTime weekAgo = musicShows[2].StartTime.AddDays(-7);
                eventToShow = $"m-countdown/{weekAgo:yyyy}{weekAgo:MM}{weekAgo:dd}";
            }
            else
            {
                eventToShow = $"m-countdown/{musicShows[2].StartTime:yyyy}{musicShows[2].StartTime:MM}{musicShows[2].StartTime:dd}";
            }
            break;
        case ConsoleKey.D4:
            select = 4;
            if (musicShows[3].StartTime.DayOfWeek != now.DayOfWeek)
            {
                DateTime weekAgo = musicShows[3].StartTime.AddDays(-7);
                eventToShow = $"music-bank/{weekAgo:yyyy}{weekAgo:MM}{weekAgo:dd}";
            }
            else
            {
                eventToShow = $"music-bank/{musicShows[3].StartTime:yyyy}{musicShows[3].StartTime:MM}{musicShows[3].StartTime:dd}";
            }
            break;
        case ConsoleKey.D5:
            select = 5;
            if (musicShows[4].StartTime.DayOfWeek != now.DayOfWeek)
            {
                DateTime weekAgo = musicShows[4].StartTime.AddDays(-7);
                eventToShow = $"show-music-core/{weekAgo:yyyy}{weekAgo:MM}{weekAgo:dd}";
            }
            else
            {
                eventToShow = $"show-music-core/{musicShows[4].StartTime:yyyy}{musicShows[4].StartTime:MM}{musicShows[4].StartTime:dd}";
            }
            break;
        case ConsoleKey.D6:
            select = 6;
            if (musicShows[5].StartTime.DayOfWeek != now.DayOfWeek)
            {
                DateTime weekAgo = musicShows[5].StartTime.AddDays(-7);
                eventToShow = $"inkigayo/{weekAgo:yyyy}{weekAgo:MM}{weekAgo:dd}";
            }
            else
            {
                eventToShow = $"inkigayo/{musicShows[5].StartTime:yyyy}{musicShows[5].StartTime:MM}{musicShows[5].StartTime:dd}";
            }
            break;
        default:
            return;
    }

    using HttpClient client = new();

    try
    {
        var result = await client.GetAsync($"https://www.reddit.com/r/kpop/wiki/music-shows/{eventToShow}.json");
        var content = JsonConvert.DeserializeObject<dynamic>(result.Content.ReadAsStringAsync().Result);
        string data = content.data.content_md;

        string _winner = data[data.IndexOf("WINNER")..];
        _winner = _winner[(_winner.IndexOf('[') + 1)..];
        _winner = _winner[.._winner.IndexOf(']')];

        string winner = _winner;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n\n{musicShows[select-1].Name} ({musicShows[select-1].EndTime:d}) winner: {winner}");
    }
    catch (Exception ex)
    {
        if (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException) // Other error causes: ?
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n\nA connection error occurred.\nPossible cause: Your or the server's network issues.");
        }
        else // Other error causes: outdated url format or outdated data source format
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n\nAn error occurred during the data reading or parsing.\nPossible cause: The selected music show didn't air last time.");
        }

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"\n({ex}");
    }
}
#endregion