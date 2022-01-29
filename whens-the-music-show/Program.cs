using System.Diagnostics;
using whens_the_music_show;

ProcessStartInfo psi = new();

// -- Changeable --
int timeZoneDifference = -7; // Change timezone difference here

psi.FileName = @"C:\Program Files\Mozilla Firefox\firefox.exe"; // Change browser location here
// ----------------

Classes TheShow = new Classes(
        "The Show", "SBS",
        Methods.SimpleTime(DayOfWeek.Tuesday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(18, 00))),
        Methods.SimpleTime(DayOfWeek.Tuesday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(19, 30)))
    );
Classes ShowChampion = new Classes(
        "Show Champion", "MBC",
        Methods.SimpleTime(DayOfWeek.Wednesday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(18, 00))),
        Methods.SimpleTime(DayOfWeek.Wednesday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(19, 30)))
    );
Classes MCountdown = new Classes(
        "M Countdown", "Mnet",
        Methods.SimpleTime(DayOfWeek.Thursday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(18, 00))),
        Methods.SimpleTime(DayOfWeek.Thursday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(19, 30)))
    );
Classes MusicBank = new Classes(
        "Music Bank", "KBS",
        Methods.SimpleTime(DayOfWeek.Friday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(17, 00))),
        Methods.SimpleTime(DayOfWeek.Friday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(18, 30)))
    );
Classes ShowMusicCore = new Classes(
        "Show! Music Core", "MBC",
        Methods.SimpleTime(DayOfWeek.Saturday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(15, 45))),
        Methods.SimpleTime(DayOfWeek.Saturday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(17, 05)))
    );
Classes Inkigayo = new Classes(
        "Inkigayo", "SBS",
        Methods.SimpleTime(DayOfWeek.Sunday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(15, 40))),
        Methods.SimpleTime(DayOfWeek.Sunday, Methods.ConvertToTimeZone(timeZoneDifference, new Time(16, 50)))
    );

Classes[] musicShows = { TheShow, ShowChampion, MCountdown, MusicBank, ShowMusicCore, Inkigayo };

List<Performance> performances = new();

ProgramData programData = new("github.com/Kim19003", 1.0);

Console.OutputEncoding = System.Text.Encoding.Default;

while (true)
{
    Methods.Header();

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
            Methods.Header();
            Methods.ShowAllMusicShows(musicShows);
            Console.ReadKey();
            break;
        case ConsoleKey.D2:
            Console.Clear();
            Methods.Header();
            Methods.ShowNextMusicShow(musicShows);
            Console.ReadKey();
            break;
        case ConsoleKey.D3:
            Console.Clear();
            Methods.Header();
            await Methods.TryGetWinner(musicShows, performances, psi);
            Console.ReadKey();
            break;
        case ConsoleKey.A:
            Console.Clear();
            Methods.Header();
            Methods.AboutTheProgram(programData);
            Console.ReadKey();
            break;
    }

    Console.Clear();
}