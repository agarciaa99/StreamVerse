using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using StreamVerse.Models;
using static Microsoft.Maui.ApplicationModel.Permissions;
using Media = StreamVerse.Models.Media;

namespace StreamVerse.Services
{
    public class TmdbService
    {
        private const string ApiKey = "c3b24aba17639d3b22d48835e931e874";
        public const string TmdbHttpClientName = "TmdbClient";
        private readonly IHttpClientFactory _httpClientFactory;

        public TmdbService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient HttpClient => _httpClientFactory.CreateClient(TmdbHttpClientName);

        private async Task<IEnumerable<Media>> GetMediasAsync(string url)
        {
            var topTenCollection = await HttpClient.GetFromJsonAsync<Movie>($"{url}&api_key={ApiKey}");
            return topTenCollection.results
                .Select(r => r.ToMediaObject());
        }

        public async Task<IEnumerable<Media>> GetTopTenAsync() =>
            await GetMediasAsync(TmdbUrls.TopTen);

        public async Task<IEnumerable<Media>> GetPopularAsync() =>
            await GetMediasAsync(TmdbUrls.Popular);

        public async Task<IEnumerable<Media>> GetAccionAsync() =>
            await GetMediasAsync(TmdbUrls.Accion);

        public async Task<IEnumerable<Media>> GetAnimacionAsync() =>
            await GetMediasAsync(TmdbUrls.Animacion);

        public async Task<(List<CastMember> actors, List<CrewMember> directors)> GetActorsAndDirectorsAsync(int movieId)
        {
            var url = $"{TmdbUrls.GetCast(movieId)}&api_key={ApiKey}";
            var response = await HttpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al obtener los créditos de la película: {response.StatusCode}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var credits = JsonSerializer.Deserialize<CreditsResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Para coincidir con los nombres en el JSON
            });

            // Ordenar actores por su orden en la película
            var actors = credits.cast.OrderBy(c => c.order).ToList();

            // Filtrar directores del equipo de producción
            var directors = credits.crew
                .Where(c => c.job.Equals("Director", StringComparison.OrdinalIgnoreCase))
                .ToList();

            return (actors, directors);
        }
    }

    public static class TmdbUrls
    {
        public const string TopTen = "3/movie/top_rated?language=es-MX";
        public const string Popular = "3/movie/popular?language=en-US";
        public const string Accion = "3/discover/movie?language=es-MX&with_genres=28";
        public const string Aventura = "3/discover/movie?language=es-MX&with_genres=12";
        public const string Animacion = "3/discover/movie?language=es-MX&with_genres=16";
        public const string Comedia = "3/discover/movie?language=es-MX&with_genres=35";
        public const string Docuemntal = "3/discover/movie?language=es-MX&with_genres=99";
        public const string Drama = "3/discover/movie?language=es-MX&with_genres=18";
        public const string Fantasia = "3/discover/movie?language=es-MX&with_genres=14";
        public const string Terror = "3/discover/movie?language=es-MX&with_genres=27";
        public const string Musica = "3/discover/movie?language=es-MX&with_genres=10402";
        public const string Misterio = "3/discover/movie?language=es-MX&with_genres=9648";
        public const string Romance = "3/discover/movie?language=es-MX&with_genres=10749";
        public const string CienciaFiccion = "3/discover/movie?language=es-MX&with_genres=878";
        public const string Suspenso = "3/discover/movie?language=es-MX&with_genres=53";
        public const string Western = "3/discover/movie?language=es-MX&with_genres=37";

        public static string GetTrailers(int movieId, string type = "movie") => $"3/{type ?? "movie"}/{movieId}/videos?language=en-US";
        public static string GetMovieDetail(int movieId, string type = "movie") => $"3/{type ?? "movie"}/{movieId}?language=es-Mx";
        public static string GetSimilar(int movieId, string type = "movie") => $"3/{type ?? "movie"}/{movieId}/similar?language=es-MX";
        public static string GetCast(int movieId, string type = "movie") => $"3/{type ?? "movie"}/{movieId}/credits?language=es-MX";
    }

    public class Movie
    {
        public int page { get; set; }
        public Result[] results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }

    public class Result
    {
        public string backdrop_path { get; set; }
        public int[] genre_ids { get; set; }
        public int id { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public string title { get; set; }
        public bool video { get; set; }
        public string media_type { get; set; } // "movie" or "tv"
        public string ThumbnailPath => poster_path ?? backdrop_path;
        public string Thumbnail => $"https://image.tmdb.org/t/p/w600_and_h900_bestv2/{ThumbnailPath}";
        public string ThumbnailSmall => $"https://image.tmdb.org/t/p/w220_and_h300_face/{ThumbnailPath}";
        public string ThumbnailUrl => $"https://image.tmdb.org/t/p/original/{ThumbnailPath}";
        public string DisplayTitle => title ?? original_title;

        public Media ToMediaObject() =>
            new()
            {
                Id = id,
                DisplayTitle = DisplayTitle,
                MediaType = media_type,
                Overview = overview,
                ReleaseDate = release_date,
                Thumbnail = Thumbnail,
                ThumbnailSmall = ThumbnailSmall,
                ThumbnailUrl = ThumbnailUrl,
            };

    }

    public class VideosWrapper
    {
        public int id { get; set; }
        public Video[] results { get; set; }
        public static Func<Video, bool> FilterTrailerTeasers => v =>
            v.official
            && v.site.Equals("Youtube", StringComparison.OrdinalIgnoreCase)
            && (v.type.Equals("Teaser", StringComparison.OrdinalIgnoreCase) || v.type.Equals("Trailer", StringComparison.OrdinalIgnoreCase));

    }

    public class Video
    {
        public string name { get; set; }
        public string key { get; set; }
        public string site { get; set; }
        public string type { get; set; }
        public bool official { get; set; }
        public DateTime published_at { get; set; }
        public string Thumbnail => $"https://i.ytimg.com/vi/{key}/mqdefault.jpg";
    }

    public class MovieDetail
    {
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public string belongs_to_collection { get; set; }
        public int budget { get; set; }
        public Genre[] genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public Production_Companies[] production_companies { get; set; }
        public Production_Countries[] production_countries { get; set; }
        public string release_date { get; set; }
        public int revenue { get; set; }
        public int runtime { get; set; }
        public Spoken_Languages[] spoken_languages { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string title { get; set; }
        public bool video { get; set; }
        public float vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class Production_Companies
    {
        public int id { get; set; }
        public string logo_path { get; set; }
        public string name { get; set; }
        public string origin_country { get; set; }
    }

    public class Production_Countries
    {
        public string iso_3166_1 { get; set; }
        public string name { get; set; }
    }

    public class Spoken_Languages
    {
        public string english_name { get; set; }
        public string iso_639_1 { get; set; }
        public string name { get; set; }
    }

    public class GenreWrapper
    {
        public IEnumerable<Genre> Genres { get; set; }
    }

    public record struct Genre(int Id, string Name);

    public class CreditsResponse
    {
        public int id { get; set; }
        public List<CastMember> cast { get; set; }
        public List<CrewMember> crew { get; set; }
    }
    public class CastMember
    {
        public int id { get; set; }
        public string name { get; set; }
        public string character { get; set; }
        public int order { get; set; }
    }

    public class CrewMember
    {
        public int id { get; set; }
        public string name { get; set; }
        public string department { get; set; }
        public string job { get; set; }
    }
}
