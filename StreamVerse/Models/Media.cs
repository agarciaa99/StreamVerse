﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamVerse.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string DisplayTitle { get; set; }
        public string MediaType { get; set; } // "movie" or "tv"
        public string Thumbnail { get; set; }
        public string ThumbnailSmall { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Overview { get; set; }
        public string ReleaseDate { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Media media && Id == media.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
