﻿using System.Collections.Generic;
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

        private Thread _listThread;

        #region Properties

        internal TaskListViewModel()
        {
            Process[] pr = Process.GetProcesses();
            _processes = new SingleProcess[pr.Length];
            for (int i = 0; i < pr.Length; ++i)
                _processes[i] = new SingleProcess(pr[i]);
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
    }
}
