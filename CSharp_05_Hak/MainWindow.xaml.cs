using System.Windows;
using System.Windows.Controls;
using CSharp_05_Hak.Tools.Navigation;
using CSharp_05_Hak.Tools.Manager;
using CSharp_05_Hak.ViewModels;


namespace CSharp_05_Hak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

    }
}
