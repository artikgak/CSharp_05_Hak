using System.Windows;
using CSharp_05_Hak.Tools;
using CSharp_05_Hak.Tools.Manager;
using CSharp_05_Hak.Tools.Navigation;

namespace CSharp_05_Hak.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel, IContentOwner
    {

        private INavigatable _content;

        #region Properties

        public INavigatable Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        #endregion

        internal MainWindowViewModel()
        {
            NavigationManager.Instance.Initialize(new InitializationNavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.Main);
        }

    }
}
