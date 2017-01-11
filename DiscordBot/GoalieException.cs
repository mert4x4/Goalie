using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class GoalieException : Exception
    {
        public GoalieException(string message) : base(message)
        {

        }

    }

}
