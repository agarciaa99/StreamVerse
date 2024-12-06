using StreamVerse.ViewModels;

namespace StreamVerse.Pages;

public partial class DetailsPage : ContentPage
{
    private readonly DetailsViewModel _viewModel;

    public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent(); 
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (width > 0)
        {
            _viewModel.SimilarItemWidth = Convert.ToInt32(width / 3) - 3;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

}