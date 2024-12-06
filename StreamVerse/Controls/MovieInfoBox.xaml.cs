using System.Windows.Input;
using StreamVerse.Models;
using StreamVerse.Pages;
using StreamVerse.ViewModels;

namespace StreamVerse.Controls;

public partial class MovieInfoBox : ContentView
{
	public static readonly BindableProperty MediaProperty =
		BindableProperty.Create("Media", typeof(Media), typeof(MovieInfoBox), null);

	public event EventHandler Closed;
	public MovieInfoBox()
	{
		InitializeComponent();
		ClosedCommand = new Command(ExecuteClosedCommand);
	}

	public Media Media
	{
		get => (Media)GetValue(MovieInfoBox.MediaProperty);
		set => SetValue(MovieInfoBox.MediaProperty, value);
	}

    public ICommand ClosedCommand { get; private set; }
    private void ExecuteClosedCommand() => Closed?.Invoke(this, EventArgs.Empty);

    private void Button_Clicked(object sender, EventArgs e) => Closed?.Invoke(this, EventArgs.Empty);

	private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		var parameters = new Dictionary<string, object>
		{
			[nameof(DetailsViewModel.Media)] = Media
		};
		await Shell.Current.GoToAsync(nameof(DetailsPage), true, parameters);
    }

    private void OnBackgroundTapped(object sender, TappedEventArgs e)
    {
        // Este m�todo consume el evento para evitar cualquier acci�n si el usuario hace tap fuera de las �reas v�lidas.
    }
}