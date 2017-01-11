using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DiscordBot
{
    

    public class GoalDetector
    {
        public event EventHandler GoalScored;

        int _myPrevGoals;


        RocketLeague myCar = new RocketLeague(1);

        public void BeginDetect()
        {
            Task.Run(async () =>
            {
                //Timer let values initialize
                await Task.Delay(3000);

                do
                {
                    _myPrevGoals = RocketLeague.teamGoals;
                    
                    await Task.Delay(500);
                    if (RocketLeague.teamGoals == _myPrevGoals + 1)
                    {
                        Console.WriteLine("[Info] Goal Scored");
                        GoalScored(this, EventArgs.Empty);
                    }


                 } while (true);

            });
        }

    }

}
