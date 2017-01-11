using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DiscordBot
{
    
    public class MemoryAccess
    {
        const int PROCESS_VM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0X0008;

        string processName { get; set; }
        IntPtr processHandle { get; set; }
        public int baseAddress { get; private set; }
        public bool detected { get; private set; }


        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);


        public MemoryAccess(string processName)
        {
            this.processName = processName;
            this.processHandle = (IntPtr)null;
            this.baseAddress = 0;
        }

        /* Detects if process with processName is running.  If it is, then it checks if processHandle and baseAddress
         * values have been set to non-zero values. If processHandle and baseAddress are 0, assigns baseAddress
         * and processHandle  */
        public bool GetProcess()
        {
            Process[] processes = Process.GetProcessesByName(processName); //Searches for Processes by name and stores in array
            if (processes.Length == 0) //If empty array is returned the process isn't found
            {
                baseAddress = 0;
                processHandle = (IntPtr)null;
                return false;
            }

            else
            {
                if (processHandle == null || baseAddress == 0)
                {
                    baseAddress = processes[0].MainModule.BaseAddress.ToInt32(); //Gets Base Address
                    processHandle = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, false, processes[0].Id); //Gets handle for requested process
                }
                return true;
            }
        }

        /* Takes in the Process handle from the GetProcess method, and reads memory at 
         * the provided lpBaseAddress with optional inclusion of offset */
        public byte[] Read(int lpBaseAddress, int[] offset = null)
        {
            offset = offset ?? new int[0];
            int bytesRead = 0;
            byte[] lpBuffer = new byte[4];

            if (offset.Length == 0) //Reading with no offset provided
            {
                if (ReadProcessMemory(processHandle, (IntPtr)lpBaseAddress, lpBuffer, lpBuffer.Length, ref bytesRead))
                {
                    return lpBuffer;
                }

                else
                {
                    return null;
                }
            }

            else // Reading with an offset provided
            {
                if (ReadProcessMemory(processHandle, (IntPtr)lpBaseAddress, lpBuffer, lpBuffer.Length, ref bytesRead))
                {
                    byte[] addressValue = lpBuffer;

                    for (int i = 0; i < offset.Length; i++)
                    {
                        int newAddress = BitConverter.ToInt32(addressValue,0) + offset[i];
                        ReadProcessMemory(processHandle, (IntPtr)newAddress, lpBuffer, lpBuffer.Length, ref bytesRead);
                        addressValue = lpBuffer;
                    }

                    return addressValue;
                }

                else
                {
                    return null;
                }
                
            }
            
        }

        /* Takes in the Process handle from the GetProcess method, and writes memory at 
         * the provided lpBaseAddress with optional inclusion of offset */
        public void Write(int lpBaseAddress, byte[] value, int[] offset = null)
        {
            offset = offset ?? new int[0];
            int bytesWritten = 0;
            byte[] lpBuffer = new byte[4];

            if (offset.Length == 0) // Writing without offset
            {
                WriteProcessMemory(processHandle, (IntPtr)lpBaseAddress, value, value.Length, ref bytesWritten);
            }

            else
            {
                if (ReadProcessMemory(processHandle, (IntPtr)lpBaseAddress, lpBuffer, lpBuffer.Length, ref bytesWritten))
                {
                    byte[] addressValue = lpBuffer;
                    int newAddress;

                    for (int i = 0; i < offset.Length; i++)
                    {
                        int b = BitConverter.ToInt32(addressValue, 0); //delete debug
                        newAddress = BitConverter.ToInt32(addressValue, 0) + offset[i];

                        if (i < offset.Length - 1)
                        {
                            ReadProcessMemory(processHandle, (IntPtr)newAddress, lpBuffer, lpBuffer.Length, ref bytesWritten);
                            addressValue = lpBuffer;
                        }

                        else
                        {
                            WriteProcessMemory(processHandle, (IntPtr)newAddress, value, value.Length, ref bytesWritten);
                        }  
                    }
                    
                }
            }
        }
    }
}
