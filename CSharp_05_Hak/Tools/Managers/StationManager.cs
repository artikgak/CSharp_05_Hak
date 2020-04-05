//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CSharp_05_Hak.Models;
//using System.Diagnostics;
//using System.Windows;

//namespace CSharp_05_Hak.Tools.Manager
//{
//    internal class StationManager
//    {
//        private static readonly object Locker = new object();
//        private static StationManager _instance;
//        private static ObservableCollection<SingleProcess> _processList;
//        internal static StationManager Instance
//        {
//            get
//            {
//                if (_instance != null)
//                {
//                    return _instance;
//                }

//                lock (Locker)
//                {
//                    return _instance ?? (_instance = new StationManager());
//                }
//            }
//        }

//        internal ObservableCollection<SingleProcess> ProcessList
//        {
//            get { return _processList; }
//        }

//        internal void Initialize()
//        {
//            _processList = new ObservableCollection<SingleProcess>();
//            UpdateProcessList();
//        }

//        private void UpdateProcessList()
//        {
//            foreach (var item in Process.GetProcesses())
//            {
//                if (item != null)
//                {
//                    _processList.Add(new SingleProcess(item));
//                }
//            }
//        }
//        private StationManager()
//        {
//        }
//    }
//}
