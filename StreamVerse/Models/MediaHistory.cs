using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamVerse.Models
{
    public class MediaHistory
    {
        public Media Media { get; set; } // Relación con el modelo Media
        public DateTime WatchedOn { get; set; } // Fecha de visualización
    }
}
