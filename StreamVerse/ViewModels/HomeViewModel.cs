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
        public ObservableCollection<Media> AdventureMovies { get; set; } = new();
        public ObservableCollection<Media> AnimationMovies { get; set; } = new();
        public ObservableCollection<Media> ComedyMovies { get; set; } = new();
        public ObservableCollection<Media> DocumentalMovies { get; set; } = new();
        public ObservableCollection<Media> DramaMovies { get; set; } = new();
        public ObservableCollection<Media> FantsyMovies { get; set; } = new();
        public ObservableCollection<Media> TerrorMovies { get; set; } = new();
        public ObservableCollection<Media> MusicMovies { get; set; } = new();
        public ObservableCollection<Media> MisteryMovies { get; set; } = new();
        public ObservableCollection<Media> RomanceMovies { get; set; } = new();
        public ObservableCollection<Media> SciFiMovies { get; set; } = new();
        public ObservableCollection<Media> SuspenseMovies { get; set; } = new();
        public ObservableCollection<Media> WesternMovies { get; set; } = new();

        public async Task InitializeAsync()
        {
            var topTenListTask = _tmdbService.GetTopTenAsync();
            var popularListTask = _tmdbService.GetPopularAsync();
            var actionListTask = _tmdbService.GetAccionAsync();
            var adventureListTask = _tmdbService.GetAventuraAsync();
            var aminationListTask = _tmdbService.GetAnimacionAsync();
            var comedyListTask = _tmdbService.GetComedyAsync();
            var documentalListTask = _tmdbService.GetDocumentalAsync();
            var dramaListTask = _tmdbService.GetDramaAsync();
            var fantasyListTask = _tmdbService.GetFantasyAsync();
            var terrorListTask = _tmdbService.GetTerrorAsync();
            var musicListTask = _tmdbService.GetMusicAsync();
            var misteryListTask = _tmdbService.GetMisteryAsync();
            var romanceListTask = _tmdbService.GetRomanceAsync();
            var scifiListTask = _tmdbService.GetSciFiAsync();
            var suspenseListTask = _tmdbService.GetSuspenseAsync();
            var westernListTask = _tmdbService.GetWesternAsync();

            var medias = await Task.WhenAll(topTenListTask,
                                            popularListTask,
                                            actionListTask,
                                            adventureListTask,
                                            aminationListTask,
                                            comedyListTask,
                                            documentalListTask,
                                            dramaListTask,
                                            fantasyListTask,
                                            terrorListTask,
                                            musicListTask,
                                            misteryListTask,
                                            romanceListTask,
                                            scifiListTask,
                                            suspenseListTask,
                                            westernListTask);

            var topTenList = medias[0];
            var popularList = medias[1];
            var actionList = medias[2];
            var adventureList = medias[3];
            var animationList = medias[4];
            var comedyList = medias[5];
            var documentalList = medias[6];
            var dramaList = medias[7];
            var fantasyList = medias[8];
            var terrorList = medias[9];
            var musicList = medias[10];
            var misteryList = medias[11];
            var romanceList = medias[12];
            var scifiList = medias[13];
            var suspenseList = medias[14];
            var westernList = medias[15];

            PopularMovie = popularList.OrderBy(p => Guid.NewGuid())
                .FirstOrDefault(p =>
                !string.IsNullOrWhiteSpace(p.DisplayTitle)
                && !string.IsNullOrWhiteSpace(p.Thumbnail));

            _popularList = medias[1];

            ChangePopularMovie();

            SetMediaCollection(topTenList, TopTenMovies);
            SetMediaCollection(popularList, PopularMovies);
            SetMediaCollection(actionList, ActionMovies);
            SetMediaCollection(adventureList, AdventureMovies);
            SetMediaCollection(animationList, AnimationMovies);
            SetMediaCollection(comedyList, ComedyMovies);
            SetMediaCollection(documentalList, DocumentalMovies);
            SetMediaCollection(dramaList, DramaMovies);
            SetMediaCollection(fantasyList, FantsyMovies);
            SetMediaCollection(terrorList, TerrorMovies);
            SetMediaCollection(musicList, MusicMovies);
            SetMediaCollection(misteryList, MisteryMovies);
            SetMediaCollection(romanceList, RomanceMovies);
            SetMediaCollection(scifiList, SciFiMovies);
            SetMediaCollection(suspenseList, SuspenseMovies);
            SetMediaCollection(westernList, WesternMovies);
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
