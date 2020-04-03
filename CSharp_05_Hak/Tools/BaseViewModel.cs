using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace CSharp_05_Hak.Tools
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        internal virtual void OnPropertyChanged([CallerMemberName] string properyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properyName));
        }
        #endregion
    }
}