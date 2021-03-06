﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CSharp_05_Hak.Models;
using CSharp_05_Hak.Tools;
using CSharp_05_Hak.Tools.MVVM;
using System.Text;

namespace CSharp_05_Hak.ViewModels.TaskList
{
    internal enum SortType
    {
        ID,
        Name,
        IsActive,
        CPUPercents,
        RAMAmount,
        RAMPercents,
        ThreadsNumber,
        User,
        FilePath,
        StartTime
    }

    internal class TaskListViewModel : BaseViewModel
    {

        #region Fields
        private static SortType currentSort = SortType.Name;
        private SingleProcess[] _processes;
        private static SingleProcess _selectedProcess;
        private ObservableCollection<SingleProcess> _newProcesses;
        #region Commands
        #region Sort
        private RelayCommand<object> _sortById;
        private RelayCommand<object> _sortByName;
        private RelayCommand<object> _sortByIsActive;
        private RelayCommand<object> _sortByCPUPercents;
        private RelayCommand<object> _sortByRAMAmount;
        private RelayCommand<object> _sortByRAMPercents;
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
        #endregion

        #region Constructor
        internal TaskListViewModel()
        {
            Process[] pr = Process.GetProcesses();
            Array.Sort(pr, delegate (Process x, Process y) { return x.ProcessName.CompareTo(y.ProcessName); });
            _newProcesses = new ObservableCollection<SingleProcess>();
            for (int i = 0; i < pr.Length; ++i)
                _newProcesses.Add(new SingleProcess(pr[i]));
            //process list
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            timer.Tick += UpdateProcessesList;
            timer.Start();
            // process meta data
            System.Timers.Timer updateProcessTimer = new System.Timers.Timer();
            updateProcessTimer.Elapsed +=
                (sender, eventArgs) => RefreshProcesses(sender, eventArgs);
            updateProcessTimer.Interval = 2000;
            updateProcessTimer.Enabled = true;
            updateProcessTimer.Start();
        }
        #endregion

        #region Properties
        public ObservableCollection<SingleProcess> NewProcesses
        {
            get { return _newProcesses; }
            private set
            {
                _newProcesses = value;
                OnPropertyChanged();
            }
        }


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

        public RelayCommand<object> ShowThreads
        {
            get
            {
                return _showThreads ?? (_showThreads = new RelayCommand<object>(
                           ShowThreadsImplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> ShowModules
        {
            get
            {
                return _showModules ?? (_showModules = new RelayCommand<object>(
                           ShowModulesImplementation, o => CanExecuteCommand()));
            }
        }

        #region SortProperties

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

        public RelayCommand<object> SortByRAMPercents
        {
            get
            {
                return _sortByRAMPercents ?? (_sortByRAMPercents = new RelayCommand<object>(o =>
                           SortImplementation(o, SortType.RAMPercents)));
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
        #endregion

        private void RefreshProcesses(object sender, EventArgs timerArguments)
        {
            lock (_newProcesses)
            {
                try
                {
                    foreach (SingleProcess process in _newProcesses)
                    {
                        try
                        {
                            process.CPUPercents = Math.Round(process.CPUCounter.NextValue() / Environment.ProcessorCount/2, 2);
                        }
                        catch (Exception) { }
                        try
                        {
                            process.RAMAmount = Math.Round(process.MemoryUsageCounter.NextValue() / 1024 / 1024 / 2, 2);
                        }
                        catch (Exception) { }
                        try
                        {
                            process.RAMPercentage = Math.Round(process.MemoryUsageCounter.NextValue() / Environment.WorkingSet/2, 2);
                        }
                        catch (Exception) { }
                        process.Threads = Process.GetProcessById(process.ID).Threads.Count;
                        process.IsActive = Process.GetProcessById(process.ID).Responding;
                    }
                }
                catch (Exception) { }
            }
        }
        private void UpdateProcessesList(object sender, EventArgs e)
        {
            var currentIds = NewProcesses.Select(p => p.ID).ToList();

            foreach (var p in Process.GetProcesses())
            {
                if (!currentIds.Remove(p.Id)) // it's a new process id
                {
                    NewProcesses.Add(new SingleProcess(p));
                }
            }

            foreach (var id in currentIds) // these do not exist any more
            {
                var process = NewProcesses.First(p => p.ID == id);
                NewProcesses.Remove(process);
            }

            SortImplementation(new object(), currentSort);
        }

        private async void SortImplementation(object obj, SortType sortType)
        {
            currentSort = sortType;
            await Task.Run(() =>
            {
                IOrderedEnumerable<SingleProcess> sortedProccesses = null;
                switch (currentSort)
                {
                    case SortType.CPUPercents:
                        sortedProccesses = from u in _newProcesses
                                           orderby u.CPUPercents
                                           select u;
                        break;
                    case SortType.FilePath:
                        sortedProccesses = from u in _newProcesses
                                           orderby u.DirectoryPath
                                           select u;
                        break;
                    case SortType.ID:
                        sortedProccesses = from u in _newProcesses
                                           orderby u.ID
                                           select u;
                        break;
                    case SortType.IsActive:
                        sortedProccesses = from u in _newProcesses
                                           orderby u.IsActive
                                           select u;
                        break;
                    case SortType.Name:
                        sortedProccesses = from u in _newProcesses
                                           orderby u.Name
                                           select u;
                        break;
                    case SortType.RAMAmount:
                        sortedProccesses = from u in _newProcesses
                                           orderby u.RAMAmount
                                           select u;
                        break;
                    case SortType.RAMPercents:
                        sortedProccesses = from u in _newProcesses
                                           orderby u.RAMPercentage
                                           select u;
                        break;
                    case SortType.StartTime:
                        sortedProccesses = from u in _newProcesses
                                           orderby u.StartTime
                                           select u;
                        break;
                    case SortType.ThreadsNumber:
                        sortedProccesses = from u in _newProcesses
                                           orderby u.Threads descending
                                           select u;
                        break;
                    default:
                        sortedProccesses = from u in _newProcesses
                                           orderby u.User
                                           select u;
                        break;
                }
                NewProcesses = new ObservableCollection<SingleProcess>(sortedProccesses);
            });
        }

        private async void EndTaskImplementation(object obj)
        {
            await Task.Run(() =>
            {
                if (_selectedProcess.checkAvailability())
                {
                    SelectedProcess?.ProcessItself?.Kill(); //_selectedProcess.ID
                    //_processes.Remove(_selectedProcess);
                    SelectedProcess = null;
                    MessageBox.Show("Successfully terminated");
                }
                else
                {
                    MessageBox.Show("Have no access");
                }
            });
        }

        private async void ShowThreadsImplementation(object obj)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                await Task.Run(() =>
                {
                    sb.Append("ID                ThreadState           PriorityLevel                           StartTime\n");
                    foreach (ProcessThread thread in SelectedProcess.ThreadsCollection)
                    {
                        sb.Append(thread.Id.ToString() + "               " + thread.ThreadState.ToString() + "                   " +
                        thread.PriorityLevel.ToString() + "                  " + thread.StartTime.ToString() + '\n');
                    }
                });
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                MessageBox.Show("Win32 Access denied", "Error");
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show("Access denied", "Error");
                return;
            }
            MessageBox.Show(sb.ToString(), "Threads");
        }

        private async void ShowModulesImplementation(object obj)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                await Task.Run(() =>
                {
                    sb.Append("ModuleName                                                                FileName\n");
                    foreach (ProcessModule module in SelectedProcess.Modules)
                    {
                        sb.Append(module.ModuleName.ToString() + "                                                   " +
                        module.FileName.ToString() + '\n');
                    }
                });
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                MessageBox.Show("Win32 Cannot access", "Error");
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot access", "Error");
                return;
            }
            MessageBox.Show(sb.ToString(), "Modules");
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
