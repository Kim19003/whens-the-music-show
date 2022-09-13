using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;

namespace whens_the_music_show
{
    internal class Methods
    {
        internal static void Header(string pageTitle)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("WHEN'S THE MUSIC SHOW!?\n");

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"> {pageTitle}\n\n");
        }

        internal static void ShowAllMusicShows(MusicShow[] musicShows)
        {
            DateTime now = DateTime.Now;

            int hoursFromNow, daysFromNow;

            for (int i = 0; i < musicShows.Length - 1; i++)
            {
                if (musicShows[i].StartTime.DayOfWeek == now.DayOfWeek)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                hoursFromNow = (musicShows[i].StartTime - now).Hours;
                daysFromNow = (musicShows[i].StartTime - now).Days;

                Console.Write($"{musicShows[i].Name} ({musicShows[i].StartTime:dddd}, {musicShows[i].StartTime.ToString("t").Replace(".", ":")}) ");
                Console.ForegroundColor = ConsoleColor.Gray;

                if (hoursFromNow < 0)
                {
                    if (daysFromNow < 0)
                    {
                        Console.WriteLine($"— {Math.Abs(daysFromNow)} day(s) and {Math.Abs(hoursFromNow)} hour(s) ago");
                    }
                    else
                    {
                        Console.WriteLine($"— {Math.Abs(hoursFromNow)} hour(s) ago");
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

            if (musicShows[^1].StartTime.DayOfWeek == now.DayOfWeek)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            hoursFromNow = (musicShows[^1].StartTime - now).Hours;
            daysFromNow = (musicShows[^1].StartTime - now).Days;

            Console.Write($"{musicShows[^1].Name} ({musicShows[^1].StartTime:dddd}, {musicShows[^1].StartTime.ToString("t").Replace(".", ":")}) ");
            Console.ForegroundColor = ConsoleColor.Gray;

            if (hoursFromNow < 0)
            {
                if (daysFromNow < 0)
                {
                    Console.WriteLine($"— {Math.Abs(daysFromNow)} day(s) and {Math.Abs(hoursFromNow)} hour(s) ago");
                }
                else
                {
                    Console.WriteLine($"— {Math.Abs(hoursFromNow)} hour(s) ago");
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

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        internal static void ShowNextMusicShow(MusicShow[] musicShows)
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

                Console.ForegroundColor = ConsoleColor.Magenta;
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
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"'{nextShow.Name}' airs next {nextShow.StartTime:dddd} at {nextShow.StartTime:t}! ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"— {daysFromNow} day(s) and {hoursFromNow} hour(s) and {minutesFromNow} minute(s) from now");
                }
                else if (now.DayOfWeek != nextShow.StartTime.DayOfWeek) // Tomorrow
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"'{nextShow.Name}' airs tomorrow ({nextShow.StartTime:dddd}) at {nextShow.StartTime:t}! ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"— {hoursFromNow} hour(s) and {minutesFromNow} minute(s) from now");
                }
                else // Today
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"'{nextShow.Name}' airs today ({nextShow.StartTime:dddd}) at {nextShow.StartTime:t}! ");
                    Console.ForegroundColor = ConsoleColor.Gray;
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

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        internal static DateTime SimpleTime(DayOfWeek dayOfWeek, Time time)
        {
            DateTime nextWeek = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hour, time.Minute, 00);

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

        internal static Time ConvertToTimeZone(int timeDifference, Time now)
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

        internal static int GetDaysToNextDay(DayOfWeek dayOfWeek)
        {
            int dayDifference = 0;

            if (DateTime.Now.DayOfWeek != dayOfWeek)
            {
                DateTime result = DateTime.Now.AddDays(1);
                dayDifference++;

                while (result.DayOfWeek != dayOfWeek)
                {
                    result = result.AddDays(1);
                    dayDifference++;
                }
            }

            return dayDifference;
        }

        internal static async Task ShowMusicShowPerformancesAndWinner(MusicShow[] musicShows, List<Performance> performances, ProcessStartInfo psi)
        {
            DateTime now = DateTime.Now;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Select the music show to show performances and winner from (within a week):\n");

            bool today = false;

            for (int i = 0; i < musicShows.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"[{i + 1}] ");

                if (now.DayOfWeek == musicShows[i].EndTime.DayOfWeek)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    today = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write($"{musicShows[i].Name} ({musicShows[i].EndTime:dddd}) ");

                Console.ForegroundColor = ConsoleColor.Gray;
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

            Console.ForegroundColor = ConsoleColor.Gray;
            ConsoleKey selection = Console.ReadKey().Key;

            int select;

            string? eventToShow;
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
                Header("Show music show performances");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Selected ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{musicShows[select - 1].Name} ");

                Console.ForegroundColor = ConsoleColor.Gray;
                if (musicShows[select - 1].StartTime.DayOfWeek != now.DayOfWeek)
                {
                    DateTime weekAgo = musicShows[select - 1].EndTime.AddDays(-7);
                    Console.Write($"(aired {weekAgo:d}, winner: ");

                }
                else
                {
                    Console.Write($"(aired today, winner: ");
                }
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write($"{GrabWinner(data ?? "")}");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($")\n\n");
                //Console.WriteLine("\n\nChoose the performance to show (favorites are highlighted):");

                GrabAndShowPerformers(performances, psi, data ?? "");
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException) // Other error causes: ?
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n\nNetwork issue occurred.");
                }
                else // Other error causes: outdated url format or outdated data source format
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n\nThe selected music show has not been added yet or the selected music show didn't air that time.");
                }

                //Console.ForegroundColor = ConsoleColor.Gray;
                //Console.WriteLine($"\n({ex}");
            }
        }

        internal static void GrabAndShowPerformers(List<Performance> performances, ProcessStartInfo psi, string data)
        {
            string[] favorites;
            try
            {
                favorites = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Favorites.txt"));
            }
            catch
            {
                favorites = Array.Empty<string>();
            }

            List<string> _performers = data.Split("|", StringSplitOptions.TrimEntries).ToList(),
            _stages = data.Split("| Song |", StringSplitOptions.TrimEntries).ToList();

            string[] stageNames = { "Special Stage", "Special Stages", "Debut Stages", "Comeback Stages", "Hot Stages" };

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
                        && !_performers[i + 1].ToLower().StartsWith("[link]")
                        && !_performers[i + 1].ToLower().StartsWith("[youtube]")
                        && !_performers[i + 1].ToLower().StartsWith("[naver]")

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
                        if (_stages[i + 1].Contains(performance.Artist ?? "(((NOT SET)))") && _stages[i + 1].Contains(performance.Song ?? "(((NOT SET)))"))
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

            char[] keys = { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };

            string currentStageName = "";

            for (int i = 0; i < performances.Count; i++)
            {
                if (performances[i].Stage != currentStageName)
                {
                    currentStageName = performances[i]?.Stage ?? "Unknown Stage";

                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (i > 0)
                    {
                        Console.WriteLine("");
                    }
                    Console.WriteLine($"{currentStageName.ToUpper()}:\n");
                }

                if (i < keys.Length)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"[{keys[i]}] ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"[(((NOT SET)))] ");
                }

                Console.ForegroundColor = ConsoleColor.White;
                if (!string.IsNullOrWhiteSpace(performances[i].Artist) && !(performances[i].Artist ?? "").Contains('ㅤ'))
                {
                    foreach (string favorite in favorites)
                    {
                        if ((performances[i].Artist ?? "(((NOT SET)))").ToLower().Contains(favorite))
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;

                            break;
                        }
                    }

                    Console.WriteLine($"{performances[i].Artist} - {performances[i].Song}");
                }
                else
                {
                    foreach (string favorite in favorites)
                    {
                        if ((performances[i - 1].Artist ?? "(((NOT SET)))").ToLower().Contains(favorite))
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;

                            break;
                        }
                    }

                    Console.WriteLine($"{performances[i - 1].Artist} - {performances[i].Song}");
                }
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
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

        internal static string GrabWinner(string data)
        {
            string winner;
            try { winner = data[data.IndexOf("WINNER\r")..]; }
            catch {
                try { winner = data[data.IndexOf("WINNER \r")..]; }
                catch {
                    try { winner = data[data.IndexOf("WINNER")..]; }
                    catch {
                        return string.Empty;
                    }
                }
            }
            winner = winner[(winner.IndexOf('[') + 1)..];
            winner = winner[..winner.IndexOf(']')];

            return winner;
        }

        internal static void AboutTheProgram(ProgramData programData)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Data source used for getting the music show data: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@"https://www.reddit.com/r/kpop/wiki/music-shows" + "\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nVersion: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{String.Format("{0:0.0}", programData.Version).Replace(',', '.')}" + "\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Creator: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{programData.Creator}" + "\n");
        }

        internal static int GetTimeZoneDifference(string timeZoneId)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTimeOffset localServerTimeOffset = DateTimeOffset.Now;
            DateTimeOffset localTimeOffset = TimeZoneInfo.ConvertTime(localServerTimeOffset, timeZoneInfo);

            return (localServerTimeOffset.DateTime - localTimeOffset.DateTime).Hours;
        }

        internal static MusicShow ConvertToMusicShow(MusicShowRaw musicShowRaw)
        {
            return new()
            {
                Name = musicShowRaw.Name,
                Organizer = musicShowRaw.Organizer,
                AirDay = musicShowRaw.AirDay,
                StartTime = new(1920, 1, 1, Convert.ToInt32(musicShowRaw.StartTime?[..musicShowRaw.StartTime.IndexOf(':')]),
                    Convert.ToInt32(musicShowRaw.StartTime?[(musicShowRaw.StartTime.IndexOf(':') + 1)..]), 0),
                EndTime = new(1920, 1, 1, Convert.ToInt32(musicShowRaw.EndTime?[..musicShowRaw.EndTime.IndexOf(':')]),
                    Convert.ToInt32(musicShowRaw.EndTime?[(musicShowRaw.EndTime.IndexOf(':') + 1)..]), 0)
            };
        }
    }
}
