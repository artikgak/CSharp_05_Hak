﻿using System;
using CSharp_05_Hak.Views;

namespace CSharp_05_Hak.Tools.Navigation
{
    internal class InitializationNavigationModel : BaseNavigationModel
    {
        public InitializationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {

        }

        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Modules:
                    ViewsDictionary.Add(viewType, new ModulesView());
                    break;
                case ViewType.Threads:
                    ViewsDictionary.Add(viewType, new ThreadsView());
                    break;
                case ViewType.Main:
                    ViewsDictionary.Add(viewType, new TaskListView());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
