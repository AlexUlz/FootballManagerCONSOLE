using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManagerCONSOLE
{
    public class Stadium
    {
        #region VARIABLES

        public int Capacity {  get; set; }
        public string Name {  get; set; }
        public Club owner {  get; set; }

        #endregion

        public Stadium(string _name, int _capacity) { }    
    }
}
