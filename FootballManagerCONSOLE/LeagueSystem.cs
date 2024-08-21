using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManagerCONSOLE
{
    public class LeagueSystem
    {
        public List<League> leagues = new List<League>();
        public string Name { get; set; }


        public LeagueSystem(int total) 
        { 
            //amount of total leagues in the system
            for(int i = 0; i < total+1; i++)
            {
                leagues.Add(new League($"Bundesliga {i+1}", 20, i+1));
            }
        }

        public void GenerateAllFixtures()
        {
            for (int i = 0;i < leagues.Count;i++)
            {
                leagues[i].GenerateFixtures();
            }
        }
        public void SimulateAllMatches()
        {
            leagues[5].SimulateMatchday();

//             for (int i= 0; i< leagues.Count; i++)
//                         {
//                             leagues[i].SimulateMatchday();
//                         }
        }
        public void AddMyClubToSystem(Club club)
        {
            if (leagues.Count > 0)
            {
                //leagues[leagues.Count - 1].RemoveClub(0); // Access the last league in the list
                leagues[leagues.Count - 1].AddClub(club);  // Add the club to the last league
            }
            else
            {
                Console.WriteLine("No leagues available to add the club.");
            }
        }
        public bool IsSeasonOver(League league)
        {
            return league.CurrentMatchday > league.TotalMatchdays;
        }

        public bool isSummerPauseOver(League league)
        {
            return league.CurrentBreakday > league.TotalBreakdays;
        }

    }

}
