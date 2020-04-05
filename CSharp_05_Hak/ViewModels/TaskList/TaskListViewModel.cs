using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using CSharp_05_Hak.Models;
using CSharp_05_Hak.Tools;

namespace CSharp_05_Hak.ViewModels.TaskList
{
    internal class TaskListViewModel: BaseViewModel
    {
        private ObservableCollection<SingleProcess> _processes;
        
        private SingleProcess _selectedProcess;

        private Thread _threadUpdateProccesses;

        #region Properties

        internal TaskListViewModel()
        {
            Process[] pr = Process.GetProcesses();
            _processes = new ObservableCollection<SingleProcess>();
            for (int i = 0; i < pr.Length; ++i)
                _processes.Add(new SingleProcess(pr[i]));
            //StartWorkingThread();
            System.Timers.Timer updateProcessParamsTimer = new System.Timers.Timer(); //Таймер обновления
            updateProcessParamsTimer.Elapsed +=
                (sender, eventArgs) => UpdateProccesses(sender, eventArgs);
            updateProcessParamsTimer.Interval = 1000;
            updateProcessParamsTimer.Enabled = true;
            updateProcessParamsTimer.Start();
        }

        public SingleProcess SelectedProcess
        {
            get {
                //_selectedProcess.
                return _selectedProcess; }
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }

        private void UpdateProccesses(object sender, EventArgs timerArguments)
        {
            Process[] pr = Process.GetProcesses();
            ObservableCollection <SingleProcess> toSet = new ObservableCollection<SingleProcess>();
            for (int i = 0; i < pr.Length; ++i)
                toSet.Add(new SingleProcess(pr[i]));
            Processes = toSet;
            //Thread.Sleep(1000);
        }

        public ObservableCollection<SingleProcess> Processes
        {
            get { return _processes; }
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }
        #endregion

        //private void StartWorkingThread()
        //{
        //    _threadUpdateProccesses = new Thread(UpdateProccesses);
        //    _threadUpdateProccesses.Start();
        //}
    }
}
