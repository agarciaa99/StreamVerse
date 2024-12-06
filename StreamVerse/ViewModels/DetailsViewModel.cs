using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StreamVerse.Models;
using StreamVerse.Pages;
using StreamVerse.Services;

namespace StreamVerse.ViewModels
{
    [QueryProperty(nameof(Media), nameof(Media))]
    public partial class DetailsViewModel : ObservableObject
    {
        private readonly TmdbService _tmdbService;
        public DetailsViewModel(TmdbService tmdbService,
                                bool isBusy = false)
        {
            _tmdbService = tmdbService;
            _isBusy = isBusy;
        }
        [ObservableProperty]
        private Media _media;

        [ObservableProperty]
        private string _mainTrailer;

        [ObservableProperty]
        private int _runtime;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private int _similarItemWidth = 125;

        public ObservableCollection<Media> Similar { get; set; } = new();


        public async Task InitializeAsync()
        {
            var similarMediasTask = _tmdbService.GetSimilarAsync(Media.Id, Media.MediaType);
            IsBusy = true;
            try
            {
                var trailerTeasersTask = _tmdbService.GetTrailersAsync(Media.Id, Media.MediaType);
                var detailsTask = _tmdbService.GetMediaDetailsAsync(Media.Id, Media.MediaType);
                

                var trailerTeasers = await trailerTeasersTask;
                var details = await detailsTask;

                if (trailerTeasers?.Any() == true)
                {
                    var trailer = trailerTeasers.FirstOrDefault(t => t.type == "Trailer");
                    if (trailer is null)
                    {
                        trailer = trailerTeasers.First();
                    }
                    MainTrailer = GenerateYoutuberUrl(trailer.key);
                }
                else
                {
                    await Shell.Current.DisplayAlert("No found", "No videos found", "Ok");
                }
                if (details is not null)
                {
                    Runtime = details.runtime;
                }
            }
            finally
            {
                IsBusy = false;
            }

            var similarMedias = await similarMediasTask;
            if (similarMedias?.Any() == true)
            {
                foreach (var media in similarMedias)
                {
                    Similar.Add(media);
                }
            }
        }

        [RelayCommand]
        private async Task ChangeToThisMedia(Media media)
        {
            var parameters = new Dictionary<string, object>
            {
                [nameof(DetailsViewModel.Media)] = Media
            };
            await Shell.Current.GoToAsync(nameof(DetailsPage), true, parameters);
        }
        private static string GenerateYoutuberUrl(string videoKey) =>
            $"https://www.youtube.com/embed/{videoKey}";
    }
}
