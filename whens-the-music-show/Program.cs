using Newtonsoft.Json;
using System.Diagnostics;
using whens_the_music_show;

ProcessStartInfo psi = new();

// -- Changeable --
int timeZoneDifference = -7; // Change timezone difference here

psi.FileName = @"C:\Program Files\Mozilla Firefox\firefox.exe"; // Change browser location here
// ----------------

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

List<Performance> performances = new();

ProgramData programData = new("github.com/Kim19003", 1.0);

Console.OutputEncoding = System.Text.Encoding.Default;

while (true)
{
    Header();

    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("(1) ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Show all music show airing times");

    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("(2) ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Show the next music show airing time");

    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("(3) ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Show music show performances and winner");

    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("(A) ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("About the program");

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
            await TryGetWinner(musicShows, performances, psi);
            Console.ReadKey();
            break;
        case ConsoleKey.A:
            Console.Clear();
            Header();
            AboutTheProgram(programData);
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

    int hoursFromNow = 0, daysFromNow = 0;

    for (int i = 0; i < musicShows.Length - 1; i++)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{musicShows[i].StartTime.DayOfWeek}: ");

        if (musicShows[i].StartTime.DayOfWeek == now.DayOfWeek)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        hoursFromNow = (musicShows[i].StartTime - now).Hours;
        daysFromNow = (musicShows[i].StartTime - now).Days;

        Console.Write($"{musicShows[i].Name} ({musicShows[i].StartTime:g}) ");
        Console.ForegroundColor = ConsoleColor.White;

        if (hoursFromNow < 0)
        {
            if (daysFromNow < 0)
            {
                Console.WriteLine($"— {Math.Abs(daysFromNow)} day(s) and {Math.Abs(hoursFromNow)} hour(s) ago\n");
            }
            else
            {
                Console.WriteLine($"— {Math.Abs(hoursFromNow)} hour(s) ago\n");
            }
        }
        else
        {
            if (daysFromNow < 1)
            {
                Console.WriteLine($"— {hoursFromNow} hour(s) from now\n");
            }
            else
            {
                Console.WriteLine($"— {daysFromNow} day(s) and {hoursFromNow} hour(s) from now\n");
            }
        }
    }

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"{musicShows[^1].StartTime.DayOfWeek}: ");

    if (musicShows[^1].StartTime.DayOfWeek == now.DayOfWeek)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
    }

    hoursFromNow = (musicShows[^1].StartTime - now).Hours;
    daysFromNow = (musicShows[^1].StartTime - now).Days;

    Console.Write($"{musicShows[^1].Name} ({musicShows[^1].StartTime:g}) ");
    Console.ForegroundColor = ConsoleColor.White;

    if (hoursFromNow < 0)
    {
        if (daysFromNow < 0)
        {
            Console.WriteLine($"— {Math.Abs(daysFromNow)} day(s) and {Math.Abs(hoursFromNow)} hour(s) ago\n");
        }
        else
        {
            Console.WriteLine($"— {Math.Abs(hoursFromNow)} hour(s) ago\n");
        }
    }
    else
    {
        if (daysFromNow < 1)
        {
            Console.WriteLine($"— {hoursFromNow} hour(s) from now");
        }
        else
        {
            Console.WriteLine($"— {daysFromNow} day(s) and {hoursFromNow} hour(s) from now");
        }
    }
}

static void ShowNextMusicShow(MusicShow[] musicShows)
{
    DateTime now = DateTime.Now;

    MusicShow? airingNow = null;
    MusicShow? nextShow = null;

    foreach (MusicShow musicShow in musicShows)
    {
        if (musicShow.StartTime.DayOfWeek < now.DayOfWeek)
        {
            continue;
        }

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

        // Since Sunday is 0 in DayOfWeek...
        else if (now.DayOfWeek == DayOfWeek.Saturday) // If it's Saturday and the music show already aired...
        {
            nextShow = musicShows[^1]; // ...the next music show is ALWAYS the Sunday's music show (last element of the array)
            break;
        }
        else if (now.DayOfWeek == DayOfWeek.Sunday) // If it's Sunday and the music show already aired...
        {
            nextShow = musicShows[0]; // ...the next music show is ALWAYS the Monday's or Tuesday's music show (first element of the array)
            break;
        }
    }

    if (airingNow != null)
    {
        int hoursAgo = (now - airingNow.StartTime).Hours;
        int minutesAgo = (now - airingNow.StartTime).Minutes;

        Console.ForegroundColor = ConsoleColor.Green;
        if (hoursAgo < 1)
        {
            Console.WriteLine($"'{airingNow.Name}' is airing right now! (started {minutesAgo} minute(s) ago)");
        }
        else
        {
            Console.WriteLine($"'{airingNow.Name}' is airing right now! (started {hoursAgo} hour(s) {minutesAgo} minute(s) ago)");
        }
    }
    else if (nextShow != null)
    {
        int hoursFromNow = (nextShow.StartTime - now).Hours;
        int minutesFromNow = (nextShow.StartTime - now).Minutes;
        int daysFromNow = (nextShow.StartTime - now).Days;

        if (daysFromNow > 1) // In two days
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"'{nextShow.Name}' airs next {nextShow.StartTime.DayOfWeek} at {nextShow.StartTime:t}! ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"— {daysFromNow} day(s) and {hoursFromNow} hour(s) and {minutesFromNow} minute(s) from now");
        }
        else if (now.DayOfWeek != nextShow.StartTime.DayOfWeek) // Tomorrow
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"'{nextShow.Name}' airs tomorrow ({nextShow.StartTime.DayOfWeek}) at {nextShow.StartTime:t}! ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"— {hoursFromNow} hour(s) and {minutesFromNow} minute(s) from now");
        }
        else // Today
        {
            Console.WriteLine((nextShow.StartTime - now).Days);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"'{nextShow.Name}' airs today ({nextShow.StartTime.DayOfWeek}) at {nextShow.StartTime:t}! ");
            Console.ForegroundColor = ConsoleColor.White;
            if (hoursFromNow < 1)
            {
                Console.WriteLine($"— {minutesFromNow} minute(s) from now");
            }
            else
            {
                Console.WriteLine($"— {hoursFromNow} hour(s) and {minutesFromNow} minute(s) from now");
            }
        }
    }
}

static DateTime SimpleTime(DayOfWeek dayOfWeek, Time time)
{
    DateTime nextWeek = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hour, time.Minute, 00);

    if (time.GetDayDifference() == 1) // Can't be more than 1 in real life
    {
        nextWeek = nextWeek.AddDays(GetDaysToNextDay(dayOfWeek) + 1);
    }
    else if (time.GetDayDifference() == -1) // Can't be less than -1 in real life
    {
        nextWeek = nextWeek.AddDays(GetDaysToNextDay(dayOfWeek) - 1);
    }
    else
    {
        nextWeek = nextWeek.AddDays(GetDaysToNextDay(dayOfWeek));
    }

    return nextWeek;
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

static int GetDaysToNextDay(DayOfWeek dayOfWeek)
{
    DateTime now = DateTime.Now;
    int dayDifference = 0;

    if (now.DayOfWeek != dayOfWeek)
    {
        DateTime result = DateTime.Now.AddDays(1);
        dayDifference++;

        while (result.DayOfWeek != dayOfWeek)
        {
            result = result.AddDays(1);
            dayDifference++;
        }

        return dayDifference;
    }

    return dayDifference;
}

static async Task TryGetWinner(MusicShow[] musicShows, List<Performance> performances, ProcessStartInfo psi)
{
    DateTime now = DateTime.Now;

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Select the music show to show performances and winner from (within a week)\n");

    bool today = false;

    for (int i = 0; i < musicShows.Length; i++)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"({i+1}) ");

        if (now.DayOfWeek == musicShows[i].EndTime.DayOfWeek)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            today = true;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        Console.Write($"{musicShows[i].Name} ({musicShows[i].EndTime.DayOfWeek}) ");
        Console.ForegroundColor = ConsoleColor.White;

        if (today)
        {
            Console.WriteLine($"— today");
            today = false;
        }
        else
        {
            DateTime weekAgo = musicShows[i].EndTime.AddDays(-7);
            Console.WriteLine($"— {Math.Abs((weekAgo - now).Days)} day(s) ago");
        }
    }

    Console.ForegroundColor = ConsoleColor.White;
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
        HttpResponseMessage result = await client.GetAsync($"https://www.reddit.com/r/kpop/wiki/music-shows/{eventToShow}.json");
        dynamic? content = JsonConvert.DeserializeObject<dynamic>(result.Content.ReadAsStringAsync().Result);
        string? data = content?.data.content_md;

        Console.Clear();
        Header();

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Selected ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{musicShows[select - 1].Name} ");

        Console.ForegroundColor = ConsoleColor.White;
        if (musicShows[select - 1].StartTime.DayOfWeek != now.DayOfWeek)
        {
            DateTime weekAgo = musicShows[select - 1].EndTime.AddDays(-7);
            Console.Write($"(aired {weekAgo:d})\n");
        }
        else
        {
            Console.Write("(aired today)\n");
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nChoose the performance to show (opens YouTube in your browser) ('Escape' or 'Enter' to end)");

        GrabAndShowPerformers(musicShows, performances, psi, now, data, select);
    }
    catch (Exception ex)
    {
        if (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException) // Other error causes: ?
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n\nConnection issues.");
        }
        else // Other error causes: outdated url format or outdated data source format
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n\nThe selected music show has not been added yet or the selected music show didn't air last time.");
        }

        //Console.ForegroundColor = ConsoleColor.Gray;
        //Console.WriteLine($"\n({ex}");
    }
}

static void GrabAndShowPerformers(MusicShow[] musicShows, List<Performance> performances, ProcessStartInfo psi, DateTime now, string data, int select)
{
    List<string> _performers = data.Split("|", StringSplitOptions.TrimEntries).ToList<string>(),
    _stages = data.Split("| Song |", StringSplitOptions.TrimEntries).ToList<string>();

    string[] stageNames = { "Special Stage", "Special Stages", "Debut Stages", "Comeback Stages",
        "Hot Stages" };

    for (int i = 0; i < _performers.Count; i++)
    {
        if (_performers[i].StartsWith("[Fancam]") || _performers[i].StartsWith("[**"))
        {
            _performers.RemoveAt(i);
        }
    }

    List<string> performers = new(), songs = new(), links = new();
    for (int i = 0; i < _performers.Count; i++)
    {
        try
        {
            if (
                // Blacklist artist names
                !_performers[i].StartsWith("[")

                // Whitelist artist names


                // Blacklist song names
                && !_performers[i + 1].StartsWith("[Link]")
                && !_performers[i + 1].StartsWith("[link]")
                && !_performers[i + 1].StartsWith("[YouTube]")
                && !_performers[i + 1].StartsWith("[Youtube]")
                && !_performers[i + 1].StartsWith("[youtube]")
                && !_performers[i + 1].StartsWith("[Naver]")
                && !_performers[i + 1].StartsWith("[naver]")

                // Whitelist song names
                && _performers[i + 1].StartsWith("[")

                // Parsing bugs occur if artist/song name starts with the excluded ones
                )
            {
                performers.Add(_performers[i]);
                songs.Add(_performers[i + 1]);
            }
        }
        catch
        {
            continue;
        }
    }

    for (int i = 0; i < songs.Count; i++)
    {
        songs[i] = songs[i].Replace("[", "").Replace("]", "");
        links.Add(songs[i][(songs[i].IndexOf("(http") + 1)..].Replace(")", ""));
        songs[i] = songs[i][..songs[i].IndexOf("(http")];
    }

    if (performances.Count > 0)
    {
        performances.Clear();
    }

    for (int i = 0; i < songs.Count; i++) // Init 'performances' list
    {
        performances.Add(new Performance(performers[i], songs[i], links[i]));
    }

    for (int i = 0; i < _stages.Count; i++)
    {
        foreach (Performance performance in performances)
        {
            try
            {
                if (_stages[i + 1].Contains(performance.Artist) && _stages[i + 1].Contains(performance.Song))
                {
                    foreach (string stageName in stageNames)
                    {
                        if (_stages[i].Contains(stageName))
                        {
                            performance.AddToStage(stageName);
                            break;
                        }
                    }
                }
            }
            catch
            {
                break;
            }
        }
    }

    Console.WriteLine("");

    GrabAndShowWinner(musicShows, now, data, select);

    char[] keys = { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };

    string currentStageName = "";

    for (int i = 0; i < performances.Count; i++)
    {
        if (performances[i].Stage != currentStageName)
        {
            currentStageName = performances[i].Stage;

            Console.ForegroundColor = ConsoleColor.Magenta;
            if (i > 0)
            {
                Console.WriteLine("");
            }
            Console.WriteLine($"-- {currentStageName} --");
        }

        if (i < keys.Length)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"({keys[i]}) ");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"(NOT SET) ");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        if (performances[i].Artist != "")
        {
            Console.WriteLine($"{performances[i].Artist} - {performances[i].Song}");
        }
        else
        {
            Console.WriteLine($"{performances[i - 1].Artist} - {performances[i].Song}");
        }
    }

    while (true)
    {
        Console.ForegroundColor = ConsoleColor.White;
        ConsoleKey selection = Console.ReadKey().Key;

        bool dontStartProcess = false;
        try
        {
            switch (selection)
            {
                case ConsoleKey.Q:
                    psi.Arguments = performances[0].Link;
                    break;
                case ConsoleKey.W:
                    psi.Arguments = performances[1].Link;
                    break;
                case ConsoleKey.E:
                    psi.Arguments = performances[2].Link;
                    break;
                case ConsoleKey.R:
                    psi.Arguments = performances[3].Link;
                    break;
                case ConsoleKey.T:
                    psi.Arguments = performances[4].Link;
                    break;
                case ConsoleKey.Y:
                    psi.Arguments = performances[5].Link;
                    break;
                case ConsoleKey.U:
                    psi.Arguments = performances[6].Link;
                    break;
                case ConsoleKey.I:
                    psi.Arguments = performances[7].Link;
                    break;
                case ConsoleKey.O:
                    psi.Arguments = performances[8].Link;
                    break;
                case ConsoleKey.P:
                    psi.Arguments = performances[9].Link;
                    break;
                case ConsoleKey.A:
                    psi.Arguments = performances[10].Link;
                    break;
                case ConsoleKey.S:
                    psi.Arguments = performances[11].Link;
                    break;
                case ConsoleKey.D:
                    psi.Arguments = performances[12].Link;
                    break;
                case ConsoleKey.F:
                    psi.Arguments = performances[13].Link;
                    break;
                case ConsoleKey.G:
                    psi.Arguments = performances[14].Link;
                    break;
                case ConsoleKey.H:
                    psi.Arguments = performances[15].Link;
                    break;
                case ConsoleKey.J:
                    psi.Arguments = performances[16].Link;
                    break;
                case ConsoleKey.K:
                    psi.Arguments = performances[17].Link;
                    break;
                case ConsoleKey.L:
                    psi.Arguments = performances[18].Link;
                    break;
                case ConsoleKey.Z:
                    psi.Arguments = performances[19].Link;
                    break;
                case ConsoleKey.X:
                    psi.Arguments = performances[20].Link;
                    break;
                case ConsoleKey.C:
                    psi.Arguments = performances[21].Link;
                    break;
                case ConsoleKey.V:
                    psi.Arguments = performances[22].Link;
                    break;
                case ConsoleKey.B:
                    psi.Arguments = performances[23].Link;
                    break;
                case ConsoleKey.N:
                    psi.Arguments = performances[24].Link;
                    break;
                case ConsoleKey.M:
                    psi.Arguments = performances[25].Link;
                    break;
                case ConsoleKey.Escape:
                    return;
                case ConsoleKey.Enter:
                    return;
                default:
                    dontStartProcess = true;
                    break;
            }

            if (!dontStartProcess)
            {
                Process.Start(psi);
                
                Thread.Sleep(100);
                IntPtr myWindowHandler = Process.GetCurrentProcess().MainWindowHandle;
                DllImport.ShowWindow(myWindowHandler, 5);
                DllImport.SetForegroundWindow(myWindowHandler);
            }
            else
            {
                dontStartProcess = false;
            }
        }
        catch // Probably selected out of the 'links' array or the selected show didn't have a link
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\nNo link available for the selected performance.");
        }
    }
}

static void GrabAndShowWinner(MusicShow[] musicShows, DateTime now, string data, int select)
{
    string _winner = data[data.IndexOf("WINNER")..];
    _winner = _winner[(_winner.IndexOf('[') + 1)..];
    _winner = _winner[.._winner.IndexOf(']')];

    string winner = _winner;

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Winner: {winner}\n");
}

static void AboutTheProgram(ProgramData programData)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("Data source used for getting the music show data: ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write(@"https://www.reddit.com/r/kpop/wiki/music-shows" + "\n");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\nVersion: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"{String.Format("{0:0.0}", programData.Version).Replace(',', '.')}" + "\n");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("Creator: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"{programData.Creator}" + "\n");
}
#endregion