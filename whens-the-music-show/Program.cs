using Newtonsoft.Json;
using System.Diagnostics;
using whens_the_music_show;

string appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");
string musicShowsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MusicShows.json");

AppSettings appSettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(appSettingsPath)) ?? throw new Exception("AppSettings can't be null!");
List<MusicShowRaw> rawMusicShows = JsonConvert.DeserializeObject<List<MusicShowRaw>>(File.ReadAllText(musicShowsPath)) ?? throw new Exception("MusicShows can't be null!");

int timeZoneDifference = Methods.GetTimeZoneDifference("Korea Standard Time");

ProcessStartInfo psi = new()
{
    FileName = appSettings.DefaultBrowserPath
};

MusicShow theShow = Methods.ConvertToMusicShow(rawMusicShows.Find(ms => ms.Name == "The Show") ?? throw new Exception("MusicShow can't be null!"));
theShow.StartTime = Methods.SimpleTime(theShow.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(theShow.StartTime.Hour, theShow.StartTime.Minute)));
theShow.EndTime = Methods.SimpleTime(theShow.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(theShow.EndTime.Hour, theShow.EndTime.Minute)));

MusicShow showChampion = Methods.ConvertToMusicShow(rawMusicShows.Find(ms => ms.Name == "Show Champion") ?? throw new Exception("MusicShow can't be null!"));
showChampion.StartTime = Methods.SimpleTime(showChampion.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(showChampion.StartTime.Hour, showChampion.StartTime.Minute)));
showChampion.EndTime = Methods.SimpleTime(showChampion.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(showChampion.EndTime.Hour, showChampion.EndTime.Minute)));

MusicShow mCountdown = Methods.ConvertToMusicShow(rawMusicShows.Find(ms => ms.Name == "M Countdown") ?? throw new Exception("MusicShow can't be null!"));
mCountdown.StartTime = Methods.SimpleTime(mCountdown.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(mCountdown.StartTime.Hour, mCountdown.StartTime.Minute)));
mCountdown.EndTime = Methods.SimpleTime(mCountdown.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(mCountdown.EndTime.Hour, mCountdown.EndTime.Minute)));

MusicShow musicBank = Methods.ConvertToMusicShow(rawMusicShows.Find(ms => ms.Name == "Music Bank") ?? throw new Exception("MusicShow can't be null!"));
musicBank.StartTime = Methods.SimpleTime(musicBank.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(musicBank.StartTime.Hour, musicBank.StartTime.Minute)));
musicBank.EndTime = Methods.SimpleTime(musicBank.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(musicBank.EndTime.Hour, musicBank.EndTime.Minute)));

MusicShow showMusicCore = Methods.ConvertToMusicShow(rawMusicShows.Find(ms => ms.Name == "Show! Music Core") ?? throw new Exception("MusicShow can't be null!"));
showMusicCore.StartTime = Methods.SimpleTime(showMusicCore.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(showMusicCore.StartTime.Hour, showMusicCore.StartTime.Minute)));
showMusicCore.EndTime = Methods.SimpleTime(showMusicCore.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(showMusicCore.EndTime.Hour, showMusicCore.EndTime.Minute)));

MusicShow inkigayo = Methods.ConvertToMusicShow(rawMusicShows.Find(ms => ms.Name == "Inkigayo") ?? throw new Exception("MusicShow can't be null!"));
inkigayo.StartTime = Methods.SimpleTime(inkigayo.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(inkigayo.StartTime.Hour, inkigayo.StartTime.Minute)));
inkigayo.EndTime = Methods.SimpleTime(inkigayo.AirDay, Methods.ConvertToTimeZone(timeZoneDifference, new(inkigayo.EndTime.Hour, inkigayo.EndTime.Minute)));

MusicShow[] musicShows = { theShow, showChampion, mCountdown, musicBank, showMusicCore, inkigayo };

List<Performance> performances = new();

ProgramData programData = new("github.com/Kim19003", 1.1);

Console.OutputEncoding = System.Text.Encoding.Default;

while (true)
{
    Methods.Header("Home");

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("[1] ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Show all music shows");

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("[2] ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Show the next music show");

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("[3] ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Show music show performances");

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("\n[A] ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("About the program");

    Console.ForegroundColor = ConsoleColor.Gray;
    ConsoleKey selection = Console.ReadKey().Key;

    switch (selection)
    {
        case ConsoleKey.D1:
            Console.Clear();
            Methods.Header("Show all music shows");
            Methods.ShowAllMusicShows(musicShows);
            Console.ReadKey();
            break;
        case ConsoleKey.D2:
            Console.Clear();
            Methods.Header("Show the next music show");
            Methods.ShowNextMusicShow(musicShows);
            Console.ReadKey();
            break;
        case ConsoleKey.D3:
            Console.Clear();
            Methods.Header("Show music show performances");
            await Methods.ShowMusicShowPerformancesAndWinner(musicShows, performances, psi);
            //Console.ReadKey();
            break;
        case ConsoleKey.A:
            Console.Clear();
            Methods.Header("About the program");
            Methods.AboutTheProgram(programData);
            Console.ReadKey();
            break;
    }

    Console.Clear();
}