using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamVerse.Models;
using System.Text.Json;

namespace StreamVerse.Services
{
    public class HistorialService
    {
        private readonly string _filePath;

        public HistorialService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "mediaHistory.json");
        }
        // Obtiene el historial completo
        public async Task<List<MediaHistory>> GetHistoryAsync()
        {
            if (!File.Exists(_filePath))
                return new List<MediaHistory>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<MediaHistory>>(json) ?? new List<MediaHistory>();
        }

        // Agrega una nueva película al historial
        public async Task AddToHistoryAsync(Media media)
        {
            var history = await GetHistoryAsync();
            var historyItem = new MediaHistory
            {
                Media = media,
                WatchedOn = DateTime.Now
            };

            history.Add(historyItem);

            var json = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
