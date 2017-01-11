using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DiscordBot
{
    public class ConfigReader
    {
        public string user { get; set; }
        public string token { get; set; }
        string line;

        public ConfigReader()
        {
            try
            {
                StreamReader config = new StreamReader("config.txt");
                while ((line = config.ReadLine()) != null)
                {
                    if (line.Contains("User="))
                    {
                        if(line.Substring(5) == "USERNAME")
                        {
                            throw new GoalieException("Default config.txt values detected, change username  & bot token values in config.txt");
                        }
                        else
                        {
                            user = line.Substring(5);
                        }
                        
                    }

                    else if (line.Contains("Token="))
                    {
                        token = line.Substring(6);
                    }
                }
            }
            catch (FileNotFoundException)
            {

                throw new GoalieException("config.txt not found in Goalie directory");
            }
            
           
            }
        }
    }

