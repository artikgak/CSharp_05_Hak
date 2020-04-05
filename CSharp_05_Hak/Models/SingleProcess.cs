using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace CSharp_05_Hak.Models
{
    internal class SingleProcess
    {
        #region Fields
        private readonly Process _process;
        private bool _isActive;
        private float _ramAmount;
        private float _cpuPercents;
        private int _threads;

        private PerformanceCounter _cpuPerformanceCounter;
        #endregion

        #region Properties
        public Process ProcessItself
        {
            get { return _process; }
        }

        public int ID
        {
            get { return _process.Id; }
        }
        public string Name
        {
            get { return _process.ProcessName; }
        }
        public bool IsActive
        {
            get { return _process.Responding; }

        }
        public float CPUPercents
        {
            get
            {
                return _cpuPercents;
            }
        }
        public float RAMAmount
        {
            get { return _ramAmount; }
        }
        public int Threads
        {
            get { return _process.Threads.Count; }
        }
        public string User
        {
            get
            {
                IntPtr processHandle = IntPtr.Zero;
                try
                {
                    OpenProcessToken(_process.Handle, 8, out processHandle);
                    WindowsIdentity wi = new WindowsIdentity(processHandle);
                    string user = wi.Name;
                    return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    if (processHandle != IntPtr.Zero)
                    {
                        CloseHandle(processHandle);
                    }
                }
            }
        }
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        public ProcessModuleCollection Modules
        {
            get
            {
                return _process.Modules;
            }
        }

        public ProcessThreadCollection ThreadsCollection
        {
            get
            {
                return _process.Threads;
            }
        }

        public string DirectoryPath
        {
            get
            {
                try
                {
                    return _process.MainModule.FileName;
                }
                catch (Exception e)
                {
                    return "Access denied";
                }
            }
        }

        public bool checkAvailability()
        {
            if (StartTime == "Access denied")
            {
                return false;
            }
            return true;
        }

        public string StartTime
        {
            get
            {
                try
                {
                    return _process.StartTime.ToString("HH:mm:ss dd/MM/yyyy");
                }
                catch (Exception e)
                {
                    return "Access denied";
                }
            }
        }
        #endregion
    
        internal SingleProcess(Process process)
        {
            _process = process;
            //perfCounter = new PerformanceCounter("Process", "% Processor Time", "chrome");
            //perfCounter = new PerformanceCounter();
            //perfCounter.NextValue();
        }

    }
}
