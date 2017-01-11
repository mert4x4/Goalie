using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class GoalScoredEventArgs : EventArgs
    {
        public RocketLeague entity { get; set; }
    }
}
