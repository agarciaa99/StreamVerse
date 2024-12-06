using StreamVerse.Services;
namespace StreamVerse.Pages;

public partial class Perfil : ContentPage
{
	public Perfil(HistorialService historialService)
	{
		InitializeComponent();
        BindingContext = historialService; // Enlaza las películas al historial
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Cambiar la raíz de la aplicación a la página de Login
        Application.Current.MainPage = new Login();
    }
}