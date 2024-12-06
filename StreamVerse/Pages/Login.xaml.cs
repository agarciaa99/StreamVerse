namespace StreamVerse.Pages;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string username = Username.Text;
        string password = Password.Text;

        if (ValidUsuario(username, password))
        {
            // Cambiar la raíz de la aplicación a AppShell
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
        }
    }

    private bool ValidUsuario(string username, string password)
    {
        var validUsers = new Dictionary<string, string>
        {
            { "arturo", "1234" },
            { "ivan", "1234" },
            { "mauricio", "2468" }
        };
        return validUsers.TryGetValue(username, out var validPassword) && validPassword == password;
    }
}