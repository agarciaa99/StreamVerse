using StreamVerse.Services;
using StreamVerse.Models;
using StreamVerse.ViewModels;

namespace StreamVerse.Pages;

public partial class MainPage : ContentPage
{
    private readonly HomeViewModel _homeViewModel;
    private readonly HistorialService _historyService;

    public MainPage(HomeViewModel homeViewModel)
    {
        InitializeComponent();
        _homeViewModel = homeViewModel;
        BindingContext = _homeViewModel;
        _historyService = new HistorialService();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _homeViewModel.InitializeAsync();
    }

    private void MovieRow_MediaSelected(object sender, Controls.MediaSelectEventArgs e)
    {
        _homeViewModel.SelectMediaCommand.Execute(e.Media);
    }

    private void MovieInfoBox_Closed(object sender, EventArgs e)
    {
        _homeViewModel.SelectMediaCommand.Execute(null);
    }









    // Llamar cuando el usuario vea una película
    private async void OnWatchMedia(Media selectedMedia)
    {
        await _historyService.AddToHistoryAsync(selectedMedia);
        await DisplayAlert("Historial", $"{selectedMedia.DisplayTitle} se ha agregado a tu historial.", "OK");

    private async void Search_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SearchPage));

    }




}
