using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManagerCONSOLE
{
    public class Match
    {
        public Club homeClub { get; set; }      
        public Club awayClub {  get; set; }
        public DateTime date { get; set; }
        public int homeGoal { get; set; }
        public int awayGoal { get; set;}
        private Random rand = new Random();

        public Match(Club _home, Club _away, DateTime _date)
        {
            homeClub = _home;
            awayClub = _away;
            date = _date;
        }

        public int[] Simulate()
        {
            homeGoal = rand.Next(0, 5);
            awayGoal = rand.Next(0, 5);
            return new[] { homeGoal, awayGoal};
        }
    }

    

}
