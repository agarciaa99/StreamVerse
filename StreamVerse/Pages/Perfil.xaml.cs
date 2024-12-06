using StreamVerse.Services;
using StreamVerse.Models;

namespace StreamVerse.Pages;

public partial class Perfil : ContentPage
{
    private readonly HistorialService _historyService;
    public Perfil(HistorialService historialService)
	{
		InitializeComponent();
        _historyService = new HistorialService();
        LoadHistory();
    }
    private async void LoadHistory()
    {
        var history = await _historyService.GetHistoryAsync();
        HistoryCollectionView.ItemsSource = history;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Cambiar la raíz de la aplicación a la página de Login
        Application.Current.MainPage = new Login();
    }
}