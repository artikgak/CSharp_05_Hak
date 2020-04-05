using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CSharp_05_Hak.Models;
using CSharp_05_Hak.Tools;
using CSharp_05_Hak.Tools.MVVM;

namespace CSharp_05_Hak.ViewModels.TaskList
{
    internal enum SortType
    {
        ID,
        Name,
        IsActive,
        CPUPercents,
        RAMAmount,
        ThreadsNumber,
        User,
        FilePath,
        StartTime
    }

    internal class TaskListViewModel: BaseViewModel
    {
        private static SortType currentSort = SortType.Name;
        private ObservableCollection<SingleProcess> _processes;
        private Process[] _proc;

        private SingleProcess _selectedProcess;

        private Thread _threadUpdateProccesses;
        #region Commands
        #region Sort
        private RelayCommand<object> _sortById;
        private RelayCommand<object> _sortByName;
        private RelayCommand<object> _sortByIsActive;
        private RelayCommand<object> _sortByCPUPercents;
        private RelayCommand<object> _sortByRAMAmount;
        private RelayCommand<object> _sortByThreadsNumber;
        private RelayCommand<object> _sortByUser;
        private RelayCommand<object> _sortByFilepath;
        private RelayCommand<object> _sortByStartingTime;
        #endregion
        private RelayCommand<object> _endTask;
        private RelayCommand<object> _openFolder;
        private RelayCommand<object> _showThreads;
        private RelayCommand<object> _showModules;
        #endregion
        internal TaskListViewModel()
        {
            Process[] pr = Process.GetProcesses();
            _processes = new ObservableCollection<SingleProcess>();
            for (int i = 0; i < pr.Length; ++i)
                _processes.Add(new SingleProcess(pr[i]));
            //StartWorkingThread();
            System.Timers.Timer updateProcessParamsTimer = new System.Timers.Timer(); //list timer
            updateProcessParamsTimer.Elapsed += UpdateProccesses;
            updateProcessParamsTimer.Interval = 2000;
            updateProcessParamsTimer.Enabled = true;
            updateProcessParamsTimer.Start();
        }

        #region Properties

        public SingleProcess SelectedProcess
        {
            get
            {
                //_selectedProcess.
                return _selectedProcess;
            }
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


        public RelayCommand<object> SortById
        {
            get
            {
                return _sortById ?? (_sortById = new RelayCommand<object>(o =>
                               SortImplementation(o, SortType.ID)));
            }
        }
        public RelayCommand<object> SortByName
        {
            get
            {
                return _sortByName ?? (_sortByName = new RelayCommand<object>(o =>
                           SortImplementation(o, SortType.Name)));
            }
        }
        public RelayCommand<object> SortByIsActive
        {
            get
            {
                return _sortByIsActive ?? (_sortByIsActive = new RelayCommand<object>(o =>
                           SortImplementation(o, SortType.IsActive)));
            }
        }
        public RelayCommand<object> SortByCPUPercents
        {
            get
            {
                return _sortByCPUPercents ?? (_sortByCPUPercents = new RelayCommand<object>(o =>
                           SortImplementation(o, SortType.CPUPercents)));
            }
        }
        public RelayCommand<object> SortByRAMAmount
        {
            get
            {
                return _sortByRAMAmount ?? (_sortByRAMAmount = new RelayCommand<object>(o =>
                           SortImplementation(o, SortType.RAMAmount)));
            }
        }
        public RelayCommand<object> SortByThreadsNumber
        {
            get
            {
                return _sortByThreadsNumber ?? (_sortByThreadsNumber = new RelayCommand<object>(o =>
                           SortImplementation(o, SortType.ThreadsNumber)));
            }
        }
        public RelayCommand<object> SortByUser
        {
            get
            {
                return _sortByUser ?? (_sortByUser = new RelayCommand<object>(o =>
                           SortImplementation(o, SortType.User)));
            }
        }
        public RelayCommand<object> SortByFilepath
        {
            get
            {
                return _sortByFilepath ?? (_sortByFilepath = new RelayCommand<object>(o =>
                           SortImplementation(o, SortType.FilePath)));
            }
        }
        public RelayCommand<object> SortByStartingTime
        {
            get
            {
                return _sortByStartingTime ?? (_sortByStartingTime = new RelayCommand<object>(o =>
                           SortImplementation(o, SortType.StartTime)));
            }
        }

        #endregion

        private async void SortImplementation(object obj, SortType sortType)
        {
            currentSort = sortType;
            await Task.Run(() =>
            {
                IOrderedEnumerable<SingleProcess> sortedProccesses;
                switch (sortType)
                {
                    case SortType.CPUPercents:
                        sortedProccesses = from u in _processes
                                        orderby u.Name
                                        select u;
                        break;
                    case SortType.FilePath:
                        sortedProccesses = from u in _processes
                                        orderby u.DirectoryPath
                                        select u;
                        break;
                    case SortType.ID:
                        sortedProccesses = from u in _processes
                                        orderby u.ID
                                        select u;
                        break;
                    case SortType.IsActive:
                        sortedProccesses = from u in _processes
                                        orderby u.IsActive
                                        select u;
                        break;
                    case SortType.Name:
                        sortedProccesses = from u in _processes
                                        orderby u.Name
                                        select u;
                        break;
                    case SortType.RAMAmount:
                        sortedProccesses = from u in _processes
                                        orderby u.RAMAmount
                                        select u;
                        break;
                    case SortType.StartTime:
                        sortedProccesses = from u in _processes
                                        orderby u.StartTime
                                        select u;
                        break;
                    case SortType.ThreadsNumber:
                        sortedProccesses = from u in _processes
                                        orderby u.Threads
                                        select u;
                        break;
                    default:
                        sortedProccesses = from u in _processes
                                        orderby u.User
                                        select u;
                        break;
                }

                Processes = new ObservableCollection<SingleProcess>(sortedProccesses);
            });
        }


        private void UpdateProccesses(object sender, EventArgs timerArguments)
        {
            Process[] pr = Process.GetProcesses();
            ObservableCollection <SingleProcess> toSet = new ObservableCollection<SingleProcess>();
            for (int i = 0; i < pr.Length; ++i)
                toSet.Add(new SingleProcess(pr[i]));
            Processes = toSet;
            SortImplementation(new object(), currentSort);
        }
    }
}
