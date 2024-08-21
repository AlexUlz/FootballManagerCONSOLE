using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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

            

            // Create the league and add the user's club
            int numberOfTeams = 20;
            int leagueLevel = 6;

            LeagueSystem bundesligaSystem = new LeagueSystem(leagueLevel-1);
            bundesligaSystem.AddMyClubToSystem(myClub);
            league = bundesligaSystem.leagues[leagueLevel-1];

            // Start the simulation loop
            DateTime gameDate = new DateTime(DateTime.Now.Year, 6, 1);
            // Generate fixtures for the league
            bundesligaSystem.GenerateAllFixtures();

            

            while (true)
            {
                while (!bundesligaSystem.IsSeasonOver(bundesligaSystem.leagues[leagueLevel-1]))
                {
                    Console.WriteLine($"Date: {gameDate:dd MMM yyyy}");

                    Console.WriteLine("Enter 'next' to simulate the next matchday, 'table' to view the league table, or 'exit' to quit:");
                    string command = Console.ReadLine().ToLower();
                    Console.Clear();

                    if (command == "next")
                    {
                        bundesligaSystem.SimulateAllMatches();
                        gameDate = gameDate.AddDays(7); // Move to the next matchday (assuming weekly matches)
                    }
                    else if (command == "table")
                    {
                        bundesligaSystem.leagues[leagueLevel-1].DisplayLeagueTable();
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine(); // Wait for user to press Enter before proceeding
                    }
                    else if (command == "exit")
                    {
                        System.Environment.Exit(1); // Exit the loop and end the game
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Please enter 'next', 'table', or 'exit'.");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                    }
                } //LEAGUE PHASE

                Console.WriteLine("Season is over!");
                Console.WriteLine("Final League Table:");
                league.DisplayLeagueTable();

                league.ShowPromotionClubs();
                league.ShowRelegationClubs();
                league.ShowPlayOffsClubs();

                while (!bundesligaSystem.isSummerPauseOver(bundesligaSystem.leagues[leagueLevel - 1])) 
                {
                    if(Console.ReadLine() == "next")
                    {
                        league.CurrentBreakday++;
                        gameDate = gameDate.AddDays(7);
                    }
                    

                } //SUMMER PAUSE PHASE

            }
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

            Console.WriteLine("AWESOME, YOUR CLUB IS NOW CREATED AND READY FOR MATCHDAY 1");

            Stadium stadium = new Stadium(stadiumName, 100);
            Club club = new Club(clubName, country, city, stadium);
            stadium.owner = club;


            return club;
        }


    }
}
