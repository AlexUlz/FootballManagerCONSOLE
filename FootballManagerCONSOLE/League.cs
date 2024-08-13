using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FootballManagerCONSOLE
{
    public class League
    {
        private Random rand = new Random();
        #region BASIC
        public string LeagueName { get; set; }
        public int NumberOfTeams { get; set; }
        public List<Club> Clubs { get; set; }
        public int CurrentSeason { get; set; }
        public int CurrentMatchday { get; set; }
        public int TotalMatchdays { get; set; }
        private List<(Club homeTeam, Club awayTeam)> Fixtures { get; set; }
        #endregion

        #region TABLE
        public Dictionary<Club, int> LeagueTable { get; set; } // Club and points
        public Dictionary<Club, int> GoalsScored { get; set; }
        public Dictionary<Club, int> GoalsConceded { get; set; }
        public Dictionary<Club, int> MatchesPlayed { get; set; }
        public Dictionary<Club, int> Wins { get; set; }
        public Dictionary<Club, int> Draws { get; set; }
        public Dictionary<Club, int> Losses { get; set; }
        #endregion

        #region FUNCTIONS
        public League(string leagueName, int numberOfTeams)
        {
            LeagueName = leagueName;
            NumberOfTeams = numberOfTeams;
            Clubs = new List<Club>();
            LeagueTable = new Dictionary<Club, int>();
            GoalsScored = new Dictionary<Club, int>();
            GoalsConceded = new Dictionary<Club, int>();
            MatchesPlayed = new Dictionary<Club, int>();
            Wins = new Dictionary<Club, int>();
            Draws = new Dictionary<Club, int>();
            Losses = new Dictionary<Club, int>();
            Fixtures = new List<(Club homeTeam, Club awayTeam)>();
            CurrentSeason = 1;
            CurrentMatchday = 1;
            TotalMatchdays = (numberOfTeams - 1) * 2; // 18 matchdays for 10 teams
        }



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
            // Use a round-robin algorithm to generate fixtures
            for (int i = 0; i < NumberOfTeams - 1; i++)
            {
                for (int j = 0; j < NumberOfTeams / 2; j++)
                {
                    Club homeTeam = Clubs[j];
                    Club awayTeam = Clubs[NumberOfTeams - 1 - j];
                    Fixtures.Add((homeTeam, awayTeam));
                }
                // Rotate clubs except the first one
                var lastClub = Clubs[NumberOfTeams - 1];
                Clubs.RemoveAt(NumberOfTeams - 1);
                Clubs.Insert(1, lastClub);
            }

            // Repeat the same for the second half of the season (switching home and away)
            var secondHalfFixtures = Fixtures.Select(f => (f.awayTeam, f.homeTeam)).ToList();
            Fixtures.AddRange(secondHalfFixtures);
        }

        public void SimulateMatchday()
        {
            if (CurrentMatchday > TotalMatchdays)
            {
                Console.WriteLine("The season is over.");
                return;
            }

            Console.WriteLine($"--- Matchday {CurrentMatchday} ---");

            var fixturesForThisMatchday = Fixtures.Skip((CurrentMatchday - 1) * NumberOfTeams / 2).Take(NumberOfTeams / 2).ToList();
            foreach (var fixture in fixturesForThisMatchday)
            {
                // Simulate match with new logic
                SimulateMatch(fixture.homeTeam, fixture.awayTeam);
            }

            CurrentMatchday++;
        }

        private void SimulateMatch(Club homeTeam, Club awayTeam)
        {
            // Ensure teams have starting eleven
            homeTeam.SetStartingEleven();
            awayTeam.SetStartingEleven();

            // Compare ratings based on tactics
            double homeAttackRating = CalculateRating(homeTeam, "F");
            double homeMidfieldRating = CalculateRating(homeTeam, "M");
            double homeDefenseRating = CalculateRating(homeTeam, "D");

            double awayAttackRating = CalculateRating(awayTeam, "F");
            double awayMidfieldRating = CalculateRating(awayTeam, "M");
            double awayDefenseRating = CalculateRating(awayTeam, "D");

            // Modify ratings based on tactics
            ApplyTacticModifiers(ref homeAttackRating, ref homeMidfieldRating, ref homeDefenseRating, homeTeam.Tactic);
            ApplyTacticModifiers(ref awayAttackRating, ref awayMidfieldRating, ref awayDefenseRating, awayTeam.Tactic);

            // Calculate chances of scoring based on ratings
            int homeGoals = CalculateGoals(homeAttackRating, awayDefenseRating);
            int awayGoals = CalculateGoals(awayAttackRating, homeDefenseRating);

            Console.WriteLine($"{homeTeam.ClubName} | {homeGoals} - {awayGoals} | {awayTeam.ClubName}");

            RecordMatchResult(homeTeam, awayTeam, homeGoals, awayGoals);
        }

        private double CalculateRating(Club team, string position)
        {
            return team.Eleven.Where(p => p.Position == position).Average(p => p.Rating);
        }

        private void ApplyTacticModifiers(ref double attackRating, ref double midfieldRating, ref double defenseRating, string tactic)
        {
            switch (tactic)
            {
                case "4-3-2-1 Attacking":
                    attackRating *= 1.3;
                    midfieldRating *= 1.0;
                    defenseRating *= 0.7;
                    break;
                case "4-3-2-1 Ball Oriented":
                    attackRating *= 1.0;
                    midfieldRating *= 1.2;
                    defenseRating *= 1.1;
                    break;
                case "4-3-2-1 Defensive":
                    attackRating *= 0.8;
                    midfieldRating *= 1.0;
                    defenseRating *= 1.4;
                    break;
                case "5-3-2 Ball Oriented":
                    attackRating *= 0.9;
                    midfieldRating *= 1.3;
                    defenseRating *= 1.2;
                    break;
                case "4-4-2 Offensive":
                    attackRating *= 1.3;
                    midfieldRating *= 1.1;
                    defenseRating *= 0.8;
                    break;
                case "4-4-2 Defensive":
                    attackRating *= 0.7;
                    midfieldRating *= 1.0;
                    defenseRating *= 1.4;
                    break;
                case "4-3-3 Attack":
                    attackRating *= 1.4;
                    midfieldRating *= 1.1;
                    defenseRating *= 0.8;
                    break;
                case "4-3-3 Defense":
                    attackRating *= 1.0;
                    midfieldRating *= 1.1;
                    defenseRating *= 1.3;
                    break;
                case "3-5-2 Offensive":
                    attackRating *= 1.2;
                    midfieldRating *= 1.3;
                    defenseRating *= 0.9;
                    break;
                case "3-5-2 Defensive":
                    attackRating *= 0.8;
                    midfieldRating *= 1.2;
                    defenseRating *= 1.4;
                    break;
                case "5-4-1 Defensive":
                    attackRating *= 0.7;
                    midfieldRating *= 1.1;
                    defenseRating *= 1.5;
                    break;
                case "5-4-1 Attacking":
                    attackRating *= 1.2;
                    midfieldRating *= 1.0;
                    defenseRating *= 1.2;
                    break;
                default:
                    // Default behavior if no tactic matches
                    break;
            }
        }



        private int CalculateGoals(double attackRating, double defenseRating)
        {
            double baseScoreChance = 0.2 + (attackRating / (attackRating + defenseRating)); // Increased base chance
            int attempts = rand.Next(1, 5); // Total goal attempts (1 to 5)
            int goals = 0;

            for (int i = 0; i < attempts; i++)
            {
                if (rand.NextDouble() < baseScoreChance)
                {
                    goals++;
                }
            }

            return goals;
        }


        public bool IsSeasonOver()
        {
            return CurrentMatchday > TotalMatchdays;
        }
        #endregion
    }
}
