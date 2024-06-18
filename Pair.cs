using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro_Nerdle
{
    public class Pair
    {
        public string Guess { get; set; }
        public string Feedback { get; set;}

        public Pair(string guess, string feedback)
        {
            this.Guess = guess;
            this.Feedback = feedback;
        }
    }
}
