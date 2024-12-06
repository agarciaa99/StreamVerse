using StreamVerse.Services;
namespace StreamVerse.Pages;

public partial class Perfil : ContentPage
{
	public Perfil(HistorialService historialService)
	{
		InitializeComponent();
        BindingContext = historialService; // Enlaza las pel�culas al historial
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Cambiar la ra�z de la aplicaci�n a la p�gina de Login
        Application.Current.MainPage = new Login();
    }
}