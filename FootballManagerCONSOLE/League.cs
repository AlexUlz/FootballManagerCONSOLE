using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballManagerCONSOLE
{
    public class League
    {
        private Random rand = new Random();

        #region BASIC

        public int Division { get; set; }
        public string LeagueName { get; set; }
        public int NumberOfTeams { get; set; }
        public List<Club> Clubs { get; set; }
        public List<Match> Matches { get; set; }
        public int CurrentSeason { get; set; }
        public int CurrentMatchday { get; set; }
        public int TotalMatchdays { get; set; }

        #endregion BASIC

        #region TABLE

        public Dictionary<Club, int> LeagueTable { get; set; } // Club and points
        public Dictionary<Club, int> GoalsScored { get; set; }
        public Dictionary<Club, int> GoalsConceded { get; set; }
        public Dictionary<Club, int> MatchesPlayed { get; set; }
        public Dictionary<Club, int> Wins { get; set; }
        public Dictionary<Club, int> Draws { get; set; }
        public Dictionary<Club, int> Losses { get; set; }

        #endregion TABLE

        public League(string leagueName, int numberOfTeams, int division)
        {
            LeagueName = leagueName;
            NumberOfTeams = numberOfTeams;
            Clubs = new List<Club>();
            Matches = new List<Match>();
            LeagueTable = new Dictionary<Club, int>();
            GoalsScored = new Dictionary<Club, int>();
            GoalsConceded = new Dictionary<Club, int>();
            MatchesPlayed = new Dictionary<Club, int>();
            Wins = new Dictionary<Club, int>();
            Draws = new Dictionary<Club, int>();
            Losses = new Dictionary<Club, int>();
            CurrentSeason = 1;
            CurrentMatchday = 1;
            TotalMatchdays = (numberOfTeams - 1) * 2;
            Division = division;

            FillLeagueWithClubs();
        }

        #region FUNCTIONS

        public void AddClub(Club club)
        {
            if (Clubs.Count < NumberOfTeams)
            {
                Clubs.Add(club);
                LeagueTable[club] = 0; // Initialize points
                GoalsScored[club] = 0;
                GoalsConceded[club] = 0;
                MatchesPlayed[club] = 0;
                Wins[club] = 0;
                Draws[club] = 0;
                Losses[club] = 0;
            }
            else
            {
                Console.WriteLine("League is full! Cannot add more clubs.");
            }
        }
        public void RemoveClub(int x)
        {
            if (x >= 0 && x < Clubs.Count)
            {
                Clubs.RemoveAt(x);
            }
            else
            {
                Console.WriteLine("Invalid index! Cannot remove the club.");
            }
        }


        public void FillLeagueWithClubs()
        {
            // Add additional random clubs to fill the league
            for (int i = 0; i < NumberOfTeams - 1; i++)
            {
                string randomClubName = GenerateRandomClubName();
                Club randomClub = new Club(randomClubName, "Germany", "City", new Stadium($"Stadium {i + 1}", 30000));
                FillSquadWithRandoms(4, 7, 7, 2, 20, Division, randomClub);

                AddClub(randomClub);
            }
        }

        public string GenerateRandomClubName()
        {
            string[] prefixes = { "FC", "SV", "VfB", "1. FC", "TSV", "SC", "SpVgg", "SSV", "Eintracht", "VfL", "1. FSV", "FSV", "TV", "Wehen" };
            string[] cityNames = { "München", "Berlin", "Hamburg", "Stuttgart", "Dortmund", "Frankfurt", "Köln", "Leipzig", "Bremen", "Nürnberg", "Essen", "Augsburg", "Bochum", "Aachen", "Hanover", "Freiburg", "Mainz", "Oberhausen", "Kaiserslautern", "Duisburg", "Wiesbaden", "Kiel", "Ingolstadt", "Düsseldorf", "Potsdam", "Weimar", "Cottbus", "Offenbach", "Regensburg" };
            string[] suffixes = { "1899", "1904", "1909", "1910", "1960", "02", "33", "60", "14", "09", "96", "99", "98", "97" };

            string prefix = prefixes[rand.Next(prefixes.Length)];
            string cityName = cityNames[rand.Next(cityNames.Length)];
            string suffix = suffixes[rand.Next(suffixes.Length)];

            int nameType = rand.Next(0, 3); // 0 = only city name, 1 = two-part name, 2 = three-part name

            switch (nameType)
            {
                case 0:
                    return $"{prefix} {cityName}"; // Prefix and city name
                case 1:
                default:
                    return $"{prefix} {cityName} {suffix}"; // Prefix, city name, and suffix
            }
        }

        public void FillSquadWithRandoms(int _f, int _m, int _d, int _g, int totalPlayers, int league, Club club)
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


        public void RecordMatchResult(Club homeTeam, Club awayTeam, int homeGoals, int awayGoals)
        {
            if (!Clubs.Contains(homeTeam) || !Clubs.Contains(awayTeam))
            {
                Console.WriteLine("One or both of the teams are not in this league.");
                return;
            }

            GoalsScored[homeTeam] += homeGoals;
            GoalsScored[awayTeam] += awayGoals;
            GoalsConceded[homeTeam] += awayGoals;
            GoalsConceded[awayTeam] += homeGoals;
            MatchesPlayed[homeTeam]++;
            MatchesPlayed[awayTeam]++;

            if (homeGoals > awayGoals)
            {
                Wins[homeTeam]++;
                Losses[awayTeam]++;
                LeagueTable[homeTeam] += 3; // 3 points for a win
            }
            else if (awayGoals > homeGoals)
            {
                Wins[awayTeam]++;
                Losses[homeTeam]++;
                LeagueTable[awayTeam] += 3;
            }
            else
            {
                Draws[homeTeam]++;
                Draws[awayTeam]++;
                LeagueTable[homeTeam] += 1; // 1 point for a draw
                LeagueTable[awayTeam] += 1;
            }
        }

        public void DisplayLeagueTable()
        {
            Console.WriteLine($"--- {LeagueName} League Table ---");
            Console.WriteLine($"{"Club",-25} {"Pld",3} {"W",3} {"D",3} {"L",3} {"GF",4} {"GA",4} {"GD",4} {"Pts",4}");
            Console.WriteLine(new string('-', 60));

            foreach (var club in LeagueTable.OrderByDescending(c => c.Value).ThenByDescending(c => GoalsScored[c.Key] - GoalsConceded[c.Key]))
            {
                int played = MatchesPlayed[club.Key];
                int wins = Wins[club.Key];
                int draws = Draws[club.Key];
                int losses = Losses[club.Key];
                int goalsFor = GoalsScored[club.Key];
                int goalsAgainst = GoalsConceded[club.Key];
                int goalDifference = goalsFor - goalsAgainst;
                int points = club.Value;

                string clubName = club.Key.ClubName.Length > 24
                    ? club.Key.ClubName.Substring(0, 24) // Truncate if too long
                    : club.Key.ClubName.PadRight(24); // Pad if too short

                Console.WriteLine($"{clubName,-25} {played,3} {wins,3} {draws,3} {losses,3} {goalsFor,4} {goalsAgainst,4} {goalDifference,4} {points,4}");
            }
        }

        public void GenerateFixtures()
        {
            Matches.Clear();
            // Use a round-robin algorithm to generate fixtures
            for (int i = 0; i < NumberOfTeams - 1; i++)
            {
                for (int j = 0; j < NumberOfTeams / 2; j++)
                {
                    Club homeTeam = Clubs[j];
                    Club awayTeam = Clubs[NumberOfTeams - 2 - j];
                    DateTime matchDate = new DateTime(CurrentSeason, 8, 1).AddDays((i * NumberOfTeams / 2 + j) * 7); // Example date logic

                    Matches.Add(new Match(homeTeam, awayTeam, matchDate));
                }
                // Rotate clubs except the first one
                var lastClub = Clubs[NumberOfTeams - 2];
                Clubs.RemoveAt(NumberOfTeams - 2);
                Clubs.Insert(1, lastClub);
            }

            // Repeat the same for the second half of the season (switching home and away)
            var secondHalfMatches = Matches.Select(m => new Match(m.awayClub, m.homeClub, m.date)).ToList();
            Matches.AddRange(secondHalfMatches);
        }

        public void SimulateMatchday()
        {
            if (CurrentMatchday > TotalMatchdays)
            {
                Console.WriteLine("The season is over.");
                return;
            }

            Console.WriteLine($"--- Matchday {CurrentMatchday} ---");

            var matchesForThisMatchday = Matches.Skip((CurrentMatchday - 1) * NumberOfTeams / 2).Take(NumberOfTeams / 2).ToList();
            foreach (var match in matchesForThisMatchday)
            {
                // Simulate match using Match class
                int[] result = match.Simulate();
                RecordMatchResult(match.homeClub, match.awayClub, result[0], result[1]);
            }

            CurrentMatchday++;
        }


        #region CLUB FINDER 
        //The champion and the second placed club will directly join the higher league
        //The third placed club in the league will play two games against the third club from the bottom from the higher league
        //The two club at the bottom will directly relegate to the lower league.
        //The third placed club from the bottom will play two games against the the third placed club from the lower league.
        public Club GetChampion()
        {
            // Return the club with the highest points
            return LeagueTable.OrderByDescending(c => c.Value)
                              .ThenByDescending(c => GoalsScored[c.Key] - GoalsConceded[c.Key])
                              .FirstOrDefault().Key;
        }

        public Club GetSecondPlaced()
        {
            return LeagueTable.OrderByDescending(c => c.Value)
                              .ThenByDescending(c => GoalsScored[c.Key] - GoalsConceded[c.Key])
                              .Skip(1) // Skip the first (champion)
                              .FirstOrDefault().Key;
        }

        public Club GetThirdPlaced()
        {
            return LeagueTable.OrderByDescending(c => c.Value)
                              .ThenByDescending(c => GoalsScored[c.Key] - GoalsConceded[c.Key])
                              .Skip(2) // Skip the first two (champion and second placed)
                              .FirstOrDefault().Key;
        }

        public Club GetLastClub()
        {
            return LeagueTable.OrderBy(c => c.Value)
                              .ThenBy(c => GoalsScored[c.Key] - GoalsConceded[c.Key])
                              .FirstOrDefault().Key; // The last place in ascending order is the first in the list
        }

        public Club GetPenultimateClub()
        {
            return LeagueTable.OrderBy(c => c.Value)
                              .ThenBy(c => GoalsScored[c.Key] - GoalsConceded[c.Key])
                              .Skip(1) // Skip the last club
                              .FirstOrDefault().Key;
        }

        public Club GetRelegationClub()
        {
            return LeagueTable.OrderBy(c => c.Value)
                              .ThenBy(c => GoalsScored[c.Key] - GoalsConceded[c.Key])
                              .Skip(2) // Skip the last two clubs
                              .FirstOrDefault().Key;
        }


        #endregion

        #endregion FUNCTIONS
    }
}