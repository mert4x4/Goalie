using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using System.IO;

namespace DiscordBot
{
    class DiscordBot
    {
        DiscordClient _client;
        public IAudioClient _vClient;
        bool botChannelConnection = false;
        string[] songs;

        Random random = new Random();

        public DiscordBot(GoalDetector goalDetector, string user, string botToken)
        {
            try
            {
                songs = Directory.GetFiles("music");   
            }

            catch (DirectoryNotFoundException)
            {
                throw new GoalieException("Music directory not found within Goalie directory");
            }

            if (songs.Length == 0)
            {
                throw new GoalieException("No usable .mp3 files in music directory");
            }

            Console.WriteLine("[Info] {0} songs loaded", songs.Length);

            //Sets up logging level
            _client = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
            });

            //Writes to log
            _client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

            //Allows use of audio
            _client.UsingAudio(x =>
            {
                x.Mode= AudioMode.Outgoing;
            });

            //If server is available and "user" is playing rocket league, will automatically join voice channel
            _client.ServerAvailable += async (s, e) =>
            {
                _client.SetGame("Rocket League");
                try
                {
                    var playing = e.Server.FindUsers(user, true).First().CurrentGame ?? new Game();
                    if (playing.Name == "Rocket League")
                    {
                        var voiceChannel = e.Server.FindUsers(user, true).First().VoiceChannel;
                        _vClient = await _client.GetService<AudioService>().Join(voiceChannel);
                        botChannelConnection = true;
                    }
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("[Error] User with name " + user + " not found on server, make sure name is correct in config.txt (Case Sensitive)");
                    Console.Read();
                    Environment.Exit(0);
                }
            
                catch (ArgumentNullException)
                {
                    Console.WriteLine("[Warning] {0} not in Voice Channel", user);
                }
                  
                         

            };

            //Detects "user" status change
            _client.UserUpdated += async (s, e) => 
            {
                var playing = e.After.CurrentGame ?? new Game();
                
                // if status change is from "user"
                if (e.Before.Name == user) 
                {
                    
                    //Starting Rocket League
                    if (playing.Name == "Rocket League" && botChannelConnection == false) 
                    {
                        var voiceChannel = e.After.VoiceChannel;
                        try
                        {
                            _vClient = await _client.GetService<AudioService>().Join(voiceChannel);
                        }
                        catch (ArgumentNullException)
                        {
                            Console.WriteLine("[Warning] {0} not in Voice Channel", user);
                        }
                        
                        botChannelConnection = true;
                    }
                    
                    //Quitting Rocket League
                    else if (playing.Name != "Rocket League" && botChannelConnection == true) 
                    {
                        {
                            await _vClient.Disconnect();
                            botChannelConnection = false;
                        };
                    }

                    // "user" switched channels
                    else if (playing.Name == "Rocket League" && botChannelConnection == true)
                    {
                        try
                        {
                            var voiceChannel = e.After.VoiceChannel.JoinAudio();
                        }
                        catch (NullReferenceException)
                        {

                            Console.WriteLine("[Warning] {0} disconnected from voice channel", user);
                        }         
                    }
                }
            };


            //Goal detection event
            goalDetector.GoalScored += (s, e) =>
            {
                int songNum = random.Next(songs.Length);
                Audio.SendAudio(songs[songNum], _client, _vClient);
            };


            //Connection set up 
            _client.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    try
                    {
                        await _client.Connect(botToken, TokenType.Bot);
                        break;
                    }
                    catch
                    {
                        await Task.Delay(3000);
                    }
                }
                
            });

        }
    }
}
