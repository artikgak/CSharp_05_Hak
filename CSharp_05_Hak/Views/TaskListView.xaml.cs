using CSharp_05_Hak.Tools.Manager;
using CSharp_05_Hak.Tools.Navigation;
using CSharp_05_Hak.ViewModels.TaskList;
using System.Windows.Controls;

namespace CSharp_05_Hak.Views
{
    /// <summary>
    /// Interaction logic for TaskListView.xaml
    /// </summary>
    public partial class TaskListView : UserControl, INavigatable
    {
        public TaskListView()
        {
            InitializeComponent();
            DataContext = new TaskListViewModel();
        }
    }
}
