namespace CSharp_05_Hak.Tools.Navigation
{
    internal enum ViewType
    {
        Main = 0,
        Modules = 1,
        Threads = 2
    }
    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}

