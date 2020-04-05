using CSharp_05_Hak.Tools.Navigation;
using CSharp_05_Hak.ViewModels.TaskList;
using System.Windows.Controls;


namespace CSharp_05_Hak.Views
{
    /// <summary>
    /// Interaction logic for ThreadsView.xaml
    /// </summary>
    public partial class ThreadsView : UserControl, INavigatable
    {
        public ThreadsView()
        {
            InitializeComponent();
            DataContext = new ThreadsViewModel();
        }
    }
}
