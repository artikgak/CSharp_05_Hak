namespace CSharp_05_Hak.Tools.Navigation
{
    internal enum ViewType
    {
        Main = 0
    }
    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}

