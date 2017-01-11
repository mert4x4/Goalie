using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Goalie! - A project by /u/garrettjones331");

            try
            {
                ConfigReader configReader = new ConfigReader();

                GoalDetector goalDetector = new GoalDetector();

                goalDetector.BeginDetect();

                DiscordBot bot = new DiscordBot(goalDetector, configReader.user, configReader.token);

            }

            catch (GoalieException e)
            {

                Console.WriteLine("[Error] " + e.Message);
                Console.Read();
            }


            

        }

    }
}
