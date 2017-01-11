using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class RocketLeague
    {
        public static bool connectionStatus;

        int _boostAddr { get; set; }
        int _xPosAddr { get; set; }
        int _goalsAddr { get; set; }
        int _scoreAddr { get; set; }
        int[] _boostOffset { get; set; }
        int[] _xPosOffset { get; set; }
        int[] _goalsOffset { get; set; }
        int[] _scoreOffset { get; set; }

        static MemoryAccess memory = new MemoryAccess("RocketLeague");

        public static int teamGoals
        {
            get
            {
                if (connectionStatus)
                {
                    try
                    {
                        return BitConverter.ToInt32(memory.Read(0x0172E084 + memory.baseAddress, new int[] { 0x5C, 0x48, 0xCC, 0x20C, 0x1F0 }), 0);
                    }
                    catch (ArgumentNullException)
                    {
                        connectionStatus = false;
                        return 0;
                    }

                }

                else
                {
                    return 0;
                }
            }

            set
            {
                if (connectionStatus)
                {
                    memory.Write(0x016A6FC0 + memory.baseAddress, BitConverter.GetBytes(value), new int[] { 0x670, 0x40, 0x38C, 0x20C, 0x1F0 });
                }
            }
        }

        public int boost
        {
            get
            {
                if (connectionStatus)
                {
                    try
                    {
                        return BitConverter.ToInt32(memory.Read(_boostAddr + memory.baseAddress, _boostOffset), 0);
                    }
                    catch (ArgumentNullException)
                    {
                        connectionStatus = false;
                        return 0;
                    }

                }

                else
                {
                    return 0;
                }
            }

            set
            {
                if (connectionStatus)
                {
                    memory.Write(_boostAddr + memory.baseAddress, BitConverter.GetBytes(value), _boostOffset);
                }
            }
        }

        public int goals
        {
            get
            {
                if (connectionStatus)
                {
                    try
                    {
                        return BitConverter.ToInt32(memory.Read(_goalsAddr + memory.baseAddress, _goalsOffset), 0);
                    }
                    catch (ArgumentNullException)
                    {
                        connectionStatus = false;
                        return 0;
                    }

                }

                else
                {
                    return 0;
                }
            }

            set
            {
                if (connectionStatus)
                {
                    memory.Write(_goalsAddr + memory.baseAddress, BitConverter.GetBytes(value), _goalsOffset);
                }
            }
        }

        public float xPos
        {
            get
            {
                if (connectionStatus)
                {
                    try
                    {
                        return BitConverter.ToSingle(memory.Read(_xPosAddr + memory.baseAddress, _xPosOffset), 0);
                    }
                    catch (ArgumentNullException)
                    {
                        connectionStatus = false;
                        return 0;
                    }

                }

                else
                {
                    return 0;
                }
            }

            set
            {
                if (connectionStatus)
                {
                    memory.Write(_xPosAddr + memory.baseAddress, BitConverter.GetBytes(value), _xPosOffset);
                }
            }
        }

        public int score
        {
            get
            {
                if (connectionStatus)
                {
                    try
                    {
                        return BitConverter.ToInt32(memory.Read(_scoreAddr + memory.baseAddress, _scoreOffset), 0);
                    }
                    catch (ArgumentNullException)
                    {
                        connectionStatus = false;
                        return 0;
                    }
                }

                else
                {
                    return 0;
                }
            }

            set
            {
                if (connectionStatus)
                {
                    memory.Write(_scoreAddr + memory.baseAddress, BitConverter.GetBytes(value), _scoreOffset);
                }
            }
        }

        public RocketLeague(int player)
        {
            Connection();
            {
                switch (player)
                {
                    case 1:
                        _boostAddr = 0x01657DF0;
                        _boostOffset = new int[] { 0x394, 0x40, 0x3C0, 0x530, 0x54 };
                        _goalsAddr = 0x016A843C;
                        _goalsOffset = new int[] { 0x358, 0x4, 0x248, 0x100, 0x724 };
                        _xPosAddr = 0x0164C660;
                        _xPosOffset = new int[] { 0x58, 0x1E8, 0x6BC, 0x0, 0x144 };
                        _scoreAddr = 0x016BBFD8;
                        _scoreOffset = new int[] { 0x4, 0x784, 0x28, 0x35C, 0x2F4 };
                        break;
                }
            }


        }

        //Returns true if the process is detected, refreshes base address if process is detected
        public static void Connection()
        {
            Task connectionStatusTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    connectionStatus = memory.GetProcess();
                    await Task.Delay(2000);
                }

            });
        }
    }
}
