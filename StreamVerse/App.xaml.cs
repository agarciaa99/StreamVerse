namespace StreamVerse
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var loginPage = new Pages.Login();
            return new Window(new NavigationPage(loginPage));
        }
    }
}