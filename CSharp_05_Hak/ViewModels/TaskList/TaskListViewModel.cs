using System.Collections.ObjectModel;
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

        public SingleProcess SelectedProcess
        {
            get { return _selectedProcess; }
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
