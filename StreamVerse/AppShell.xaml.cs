﻿using StreamVerse.Pages;

namespace StreamVerse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
        }
    }
}
