using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FootballManagerCONSOLE
{
    public class Club
    {
        #region BASIC
        public string ClubName { get; set; }
        public Stadium stadium { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Tactic { get; set; }


        #endregion

        #region LEAGUE
        public string LeagueName { get; set; }
        public int CurrentLeaguePosition { get; set; }

        #endregion

        #region FINANCE
        public decimal ClubBudget { get; set; }
        public decimal WageBudget { get; set; }
        public decimal TransferBudget { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal StadiumRevenue { get; set; }
        public decimal SponsorshipRevenue { get; set; }
        public decimal PlayerWages { get; set; }

        #endregion

        #region SQUAD
        const int maxPlayers = 50;
        public List<Player> Squad = new List<Player>();
        public List<Player> Eleven = new List<Player>(10);


        #endregion

        #region MANAGMENT
        public string ManagerName { get; set; }
        public string AssistantManagerName { get; set; }
        public List<string> CoachingStaff { get; set; }

        #endregion

        #region HISTORY
        public int YearFounded { get; set; }
        public List<string> TrophiesWon { get; set; }

        #endregion

        #region TRANSFER
        public List<Player> TransferTargets { get; set; }
        public List<Player> PlayersForSale { get; set; }

        #endregion

        #region FANS
        public int FanbaseSize { get; set; }
        public string RivalClub { get; set; }

        #endregion
       
        public Club(string _ClubName, string _Country, string _City, Stadium _stadium)
        {
            ClubName = _ClubName;
            Country = _Country;
            City = _City;
            stadium = _stadium;
        }

        public void AddPlayer(Player player)
        {
            if (Squad.Count < maxPlayers)
            {
                Squad.Add(player);
            }
            else
            {
                Console.WriteLine("Squad is full! Remove a player before adding a new one.");
            }
        }

        public void RemovePlayer(Player player)
        {
            Squad.Remove(player);
        }

        public void ShowAllPlayers()
        {
            for(int i = 0; i< Squad.Count; i++)
            {
                Console.WriteLine("Age, RAge, Pos, Value, Rating, Name");
                Console.WriteLine("{0}: {1}: {2}: {3}: {4}: {5}", Squad[i].Age, Squad[i].RetireAge, Squad[i].Position, Squad[i].Value, Squad[i].Rating, Squad[i].Name);
            }
        }


        

        // Constructor
        public Club(string clubName)
        {
            ClubName = clubName;
            Squad = new List<Player>();
            Eleven = new List<Player>();
        }

        // Function to set the starting eleven based on tactics (this could be more advanced)
        public void SetStartingEleven()
        {
            // For now, just taking the first 11 players (you could add logic to choose best players for positions)
            Eleven = Squad.OrderByDescending(p => p.Rating).Take(11).ToList();
        }


    }

}
