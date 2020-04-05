using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CSharp_05_Hak.Tools.Navigation;
using CSharp_05_Hak.ViewModels.TaskList;

namespace CSharp_05_Hak.Views
{
    /// <summary>
    /// Interaction logic for ModulesView.xaml
    /// </summary>
    public partial class ModulesView : UserControl, INavigatable
    {
        public ModulesView()
        {
            InitializeComponent();
            DataContext = new ModulesViewModel();
        }
    }
}
