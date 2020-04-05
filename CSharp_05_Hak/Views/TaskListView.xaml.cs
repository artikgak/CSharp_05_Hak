using CSharp_05_Hak.Tools.Navigation;
using CSharp_05_Hak.ViewModels.TaskList;
using System.Windows.Controls;

namespace CSharp_05_Hak.Views
{
    /// <summary>
    /// Interaction logic for TaskListView.xaml
    /// </summary>
    internal partial class TaskListView : UserControl, INavigatable
    {
        internal TaskListView()
        {
            InitializeComponent();
            DataContext = new TaskListViewModel();
        }
    }
}
