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
        private SingleProcess[] _processes;
        private System.Timers.Timer updateProcessParamsTimer;
        private SingleProcess _selectedProcess;

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
            Array.Sort(pr,
                                    delegate (Process x, Process y) { return x.ProcessName.CompareTo(y.ProcessName); });
            Processes = new SingleProcess[pr.Length];
            for (int i = 0; i < pr.Length; ++i)
                Processes[i] = new SingleProcess(pr[i]);
            updateProcessParamsTimer = new System.Timers.Timer(); //list timer
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
                return _selectedProcess;
            }
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }
        public SingleProcess[] Processes
        {
            get { return _processes; }
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> EndTask
        {
            get
            {
                return _endTask ?? (_endTask = new RelayCommand<object>(
                           EndTaskImplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> OpenFolder
        {
            get
            {
                return _openFolder ?? (_openFolder = new RelayCommand<object>(
                           OpenFolderImplementation, o => CanExecuteCommand()));
            }
        }

        //public RelayCommand<object> ShowThreads
        //{
        //    get
        //    {
        //        return _showThreads ?? (_showThreads = new RelayCommand<object>(
        //                   ShowThreadsImplementation, o => CanExecuteCommand()));
        //    }
        //}

        //public RelayCommand<object> ShowModules
        //{
        //    get
        //    {
        //        return _showModules ?? (_showModules = new RelayCommand<object>(
        //                   ShowModulesImplementation, o => CanExecuteCommand()));
        //    }
        //}

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

        private void SortImplementation(object obj, SortType sortType)
        {
            currentSort = sortType;
            UpdateProccesses(new object(), new EventArgs());
        }

        //private async void SortImplementation1(object obj, SortType sortType)
        //{
        //    currentSort = sortType;
        //    await Task.Run(() =>
        //    {
        //        IOrderedEnumerable<SingleProcess> sortedProccesses;
        //        switch (sortType)
        //        {
        //            case SortType.CPUPercents:
        //                Array.Sort(Processes,
        //                            delegate (SingleProcess x, SingleProcess y) { return x.CPUPercents.CompareTo(y.CPUPercents); });
        //                break;
        //            case SortType.FilePath:
        //                sortedProccesses = from u in _processes
        //                                orderby u.DirectoryPath
        //                                select u;
        //                break;
        //            case SortType.ID:
        //                Array.Sort(_processes,
        //                            delegate (SingleProcess x, SingleProcess y) { return x.ID.CompareTo(y.ID); });
        //                break;
        //            case SortType.IsActive:
        //                sortedProccesses = from u in _processes
        //                                orderby u.IsActive
        //                                select u;
        //                break;
        //            case SortType.Name:
        //                sortedProccesses = from u in _processes
        //                                orderby u.Name
        //                                select u;
        //                break;
        //            case SortType.RAMAmount:
        //                sortedProccesses = from u in _processes
        //                                orderby u.RAMAmount
        //                                select u;
        //                break;
        //            case SortType.StartTime:
        //                sortedProccesses = from u in _processes
        //                                orderby u.StartTime
        //                                select u;
        //                break;
        //            case SortType.ThreadsNumber:
        //                sortedProccesses = from u in _processes
        //                                orderby u.Threads
        //                                select u;
        //                break;
        //            default:
        //                sortedProccesses = from u in _processes
        //                                orderby u.User
        //                                select u;
        //                break;
        //        }
        //    });
        //}


        private void UpdateProccesses(object sender, EventArgs timerArguments)
        {
            Process[] pr = Process.GetProcesses();
            switch (currentSort)
            {
                case SortType.Name:
                    Array.Sort(pr, delegate (Process x, Process y) { return x.ProcessName.CompareTo(y.ProcessName); });
                    break;
                case SortType.ID:
                Array.Sort(pr, delegate (Process x, Process y) { return x.Id.CompareTo(y.Id); });
                break;
                case SortType.CPUPercents:
                    //Array.Sort(Processes,delegate (SingleProcess x, SingleProcess y) { return x.CPUPercents.CompareTo(y.CPUPercents); });
                    break;
                case SortType.FilePath:
                    Array.Sort(pr, delegate (Process x, Process y) { return x.MainModule.FileName.CompareTo(y.MainModule.FileName); });
                    break;
                case SortType.IsActive:
                    Array.Sort(pr, delegate (Process x, Process y) { return x.Responding.CompareTo(y.Responding); });
                    break;
                case SortType.RAMAmount:
                    //Array.Sort(pr, delegate (Process x, Process y) { return x.ProcessName.CompareTo(y.ProcessName); });
                    break;
                case SortType.StartTime:
                    //check non null
                    //Array.Sort(pr, delegate (Process x, Process y) { return x.StartTime.CompareTo(y.StartTime); });
                    break;
                case SortType.ThreadsNumber:
                    Array.Sort(pr, delegate (Process x, Process y) { return x.Threads.Count.CompareTo(y.Threads.Count); });
                    break;
                default: //User
                    //Array.Sort(pr, delegate (Process x, Process y) { return x.ProcessName.CompareTo(y.ProcessName); });
                    break;
            }
            Processes = new SingleProcess[pr.Length];
            for (int i = 0; i < pr.Length; ++i)
                Processes[i] = new SingleProcess(pr[i]);
            //SortImplementation(new object(), currentSort);
        }

        private async void EndTaskImplementation(object obj)
        {
            await Task.Run(() => {
                if (_selectedProcess.checkAvailability())
                {
                    SelectedProcess?.ProcessItself?.Kill(); //_selectedProcess.ID
                    //_processes.Remove(_selectedProcess);
                    SelectedProcess = null;
                    MessageBox.Show("Successfully deleted");
                }
                else
                {
                    MessageBox.Show("Have no access");
                }
            });
        }

        private void OpenFolderImplementation(object obj)
        {
            try
            {
                Process.Start("explorer.exe",
                    _selectedProcess.DirectoryPath.Substring(0,
                        _selectedProcess.DirectoryPath.LastIndexOf("\\", StringComparison.Ordinal)));
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while accessing processing data");
            }
        }

        private bool CanExecuteCommand()
        {
            return SelectedProcess != null;
        }

    }
}
