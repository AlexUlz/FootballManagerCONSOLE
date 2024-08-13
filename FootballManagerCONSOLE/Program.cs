using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballManagerCONSOLE
{
    class Program
    {
        static Club myClub;
        static League league;
        static Random rand = new Random();

        static void Main(string[] args)
        {
            // Phase 01: Create a Club!
            myClub = CreateAClub();

            // Phase 02: Fill the Squad with random Players
            int leagueLevel = 6;
            FillSquadWithRandoms(4, 7, 7, 2, 20, leagueLevel, myClub);

            // Create the league and add the user's club
            int numberOfTeams = 18;
            league = new League("Bundesliga", numberOfTeams); // Renamed to "Bundesliga" for realism
            league.AddClub(myClub);

            // Add additional random clubs to fill the league
            for (int i = 0; i < numberOfTeams - 1; i++)
            {
                string randomClubName = GenerateRandomClubName();
                Club randomClub = new Club(randomClubName, "Germany", "City", new Stadium($"Stadium {i + 1}", 30000));
                FillSquadWithRandoms(4, 7, 7, 2, 20, leagueLevel, randomClub);

                // Assign a random tactic to the AI-generated club
                randomClub.Tactic = SelectRandomTactic();

                league.AddClub(randomClub);
            }

            // Generate fixtures for the league
            league.GenerateFixtures();

            // Start the simulation loop
            DateTime gameDate = new DateTime(DateTime.Now.Year, 6, 1);

            while (!league.IsSeasonOver())
            {
                Console.WriteLine($"Date: {gameDate:dd MMM yyyy}");

                Console.WriteLine("Enter 'next' to simulate the next matchday, 'table' to view the league table, or 'exit' to quit:");
                string command = Console.ReadLine().ToLower();
                Console.Clear();

                if (command == "next")
                {
                    league.SimulateMatchday();
                    gameDate = gameDate.AddDays(7); // Move to the next matchday (assuming weekly matches)
                }
                else if (command == "table")
                {
                    league.DisplayLeagueTable();
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine(); // Wait for user to press Enter before proceeding
                }
                else if (command == "exit")
                {
                    break; // Exit the loop and end the game
                }
                else
                {
                    Console.WriteLine("Invalid command. Please enter 'next', 'table', or 'exit'.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Season is over!");
            Console.WriteLine("Final League Table:");
            league.DisplayLeagueTable();
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        public static Club CreateAClub()
        {
            Console.WriteLine("WELCOME TO FOOTBALL MANAGER CONSOLE EDITION");
            Console.WriteLine("CREATE YOUR CLUB!");

            Console.Write("LET'S BEGIN WITH THE CLUB NAME: ");
            string clubName = Console.ReadLine();
            Console.Write("GREAT, AND WHAT'S THE NAME OF YOUR STADIUM: ");
            string stadiumName = Console.ReadLine();
            Console.Write("GOOD CHOICE, IN WHICH COUNTRY IS YOUR CLUB LOCATED: ");
            string country = Console.ReadLine();
            Console.Write("OK, AND IN WHICH CITY: ");
            string city = Console.ReadLine();

            // Let the user choose a tactic
            string selectedTactic = ChooseTactic();

            Console.WriteLine("AWESOME, YOUR CLUB IS NOW CREATED AND READY FOR MATCHDAY 1");

            Stadium stadium = new Stadium(stadiumName, 100);
            Club club = new Club(clubName, country, city, stadium);
            stadium.owner = club;

            // Set the chosen tactic
            club.Tactic = selectedTactic;

            return club;
        }

        public static string ChooseTactic()
        {
            // List of available tactics
            List<string> tactics = new List<string>
            {
                "4-3-2-1 Attacking",
                "4-3-2-1 Ball Oriented",
                "4-3-2-1 Defensive",
                "5-3-2 Ball Oriented",
                "4-4-2 Offensive",
                "4-4-2 Defensive",
                "4-3-3 Attack",
                "4-3-3 Defense",
                "3-5-2 Offensive",
                "3-5-2 Defensive",
                "5-4-1 Defensive",
                "5-4-1 Attacking"
            };

            Console.WriteLine("CHOOSE YOUR TACTIC:");
            for (int i = 0; i < tactics.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tactics[i]}");
            }

            int choice;
            do
            {
                Console.Write("Enter the number of your chosen tactic: ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > tactics.Count);

            return tactics[choice - 1];
        }

        public static string SelectRandomTactic()
        {
            // List of available tactics
            List<string> tactics = new List<string>
            {
                "4-3-2-1 Attacking",
                "4-3-2-1 Ball Oriented",
                "4-3-2-1 Defensive",
                "5-3-2 Ball Oriented",
                "4-4-2 Offensive",
                "4-4-2 Defensive",
                "4-3-3 Attack",
                "4-3-3 Defense",
                "3-5-2 Offensive",
                "3-5-2 Defensive",
                "5-4-1 Defensive",
                "5-4-1 Attacking"
            };

            return tactics[rand.Next(tactics.Count)];
        }

        public static void FillSquadWithRandoms(int _f, int _m, int _d, int _g, int totalPlayers, int league, Club club)
        {
            for (int f = 0; f < _f; f++) // Creates only Forwards
            {
                Player player = new Player(league);
                player.Position = "F";
                club.AddPlayer(player);
            }
            for (int m = 0; m < _m; m++) // Creates only Midfielders
            {
                Player player = new Player(league);
                player.Position = "M";
                club.AddPlayer(player);
            }
            for (int d = 0; d < _d; d++) // Creates only Defenders
            {
                Player player = new Player(league);
                player.Position = "D";
                club.AddPlayer(player);
            }
            for (int g = 0; g < _g; g++) // Creates only Goalkeepers
            {
                Player player = new Player(league);
                player.Position = "G";
                club.AddPlayer(player);
            }
        }

        // Generate a random club name with dynamic length
        public static string GenerateRandomClubName()
        {
            string[] prefixes = { "FC", "SV", "VfB", "1. FC", "TSV", "SC", "SpVgg", "SSV", "Eintracht", "VfL", "1. FSV", "FSV", "TV", "Wehen" };
            string[] cityNames = { "München", "Berlin", "Hamburg", "Stuttgart", "Dortmund", "Frankfurt", "Köln", "Leipzig", "Bremen", "Nürnberg", "Essen", "Augsburg", "Bochum", "Aachen", "Hanover", "Freiburg", "Mainz", "Oberhausen", "Kaiserslautern", "Duisburg", "Wiesbaden", "Kiel", "Ingolstadt", "Düsseldorf", "Potsdam", "Weimar", "Cottbus", "Offenbach", "Regensburg" };
            string[] suffixes = { "1899", "1904", "1909", "1910", "1960", "02", "33", "60", "14", "09", "96", "99" };

            string prefix = prefixes[rand.Next(prefixes.Length)];
            string cityName = cityNames[rand.Next(cityNames.Length)];
            string suffix = suffixes[rand.Next(suffixes.Length)];

            int nameType = rand.Next(2); // 0 = only city name, 1 = two-part name, 2 = three-part name

            switch (nameType)
            {
                case 0:
                    return $"{prefix} {cityName}"; // Prefix and city name
                case 1:
                default:
                    return $"{prefix} {cityName} {suffix}"; // Prefix, city name, and suffix
            }
        }
    }
}
