using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using StreamVerse.Models;
using System.Collections.ObjectModel;
using StreamVerse.Services;
using System.Timers;

namespace StreamVerse.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly TmdbService _tmdbService;
        private readonly System.Timers.Timer _movieTimer;
        private IEnumerable<Media> _popularList;
        public HomeViewModel(TmdbService tmdbService)
        {
            _tmdbService = tmdbService;

            _movieTimer = new System.Timers.Timer(10000); // Cambiar cada 5 segundos
            _movieTimer.Elapsed += (s, e) => ChangePopularMovie();
            _movieTimer.AutoReset = true; // Asegura que el temporizador se reinicie automáticamente
            _movieTimer.Enabled = true; // Asegura que el temporizador esté habilitado
        }

        [ObservableProperty]
        private Media _popularMovie;

        public ObservableCollection<Media> TopTenMovies { get; set; } = new();
        public ObservableCollection<Media> PopularMovies { get; set; } = new();
        public ObservableCollection<Media> ActionMovies { get; set; } = new();
        public ObservableCollection<Media> AnimationMovies { get; set; } = new();

        public async Task InitializeAsync()
        {
            var topTenListTask = _tmdbService.GetTopTenAsync();
            var popularListTask = _tmdbService.GetPopularAsync();
            var actionListTask = _tmdbService.GetAccionAsync();
            var aminationListTask = _tmdbService.GetAnimacionAsync();

            var medias = await Task.WhenAll(topTenListTask,
                                            popularListTask,
                                            actionListTask,
                                            aminationListTask);

            var topTenList = medias[0];
            var popularList = medias[1];
            var actionList = medias[2];
            var animationList = medias[3];

            PopularMovie = popularList.OrderBy(p => Guid.NewGuid())
                .FirstOrDefault(p =>
                !string.IsNullOrWhiteSpace(p.DisplayTitle)
                && !string.IsNullOrWhiteSpace(p.Thumbnail));

            _popularList = medias[1];

            ChangePopularMovie();

            SetMediaCollection(topTenList, TopTenMovies);
            SetMediaCollection(popularList, PopularMovies);
            SetMediaCollection(actionList, ActionMovies);
            SetMediaCollection(animationList, AnimationMovies);
        }

        private void ChangePopularMovie()
        {
            if (_popularList == null || !_popularList.Any())
                return;

            // Cambiar a una película aleatoria de la lista popular
            PopularMovie = _popularList.OrderBy(r => Guid.NewGuid())
                .FirstOrDefault(r =>
                    !string.IsNullOrWhiteSpace(r.DisplayTitle)
                    && !string.IsNullOrWhiteSpace(r.Thumbnail));
        }

        private static void SetMediaCollection(IEnumerable<Media> medias, ObservableCollection<Media> collection)
        {
            collection.Clear();
            foreach (var media in medias)
            {
                collection.Add(media);
            }
        }
    }
}
