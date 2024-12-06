using StreamVerse.Models;
using StreamVerse.Pages;
using StreamVerse.ViewModels;

namespace StreamVerse.Pages;

public partial class SearchPage : ContentPage
{
    private readonly SearchViewModel _searchViewModel;

    public SearchPage(SearchViewModel searchViewModel)
    {
        InitializeComponent();
        _searchViewModel = searchViewModel;
        BindingContext = _searchViewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        //await _searchViewModel.InitializeAsync();
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
