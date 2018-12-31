using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace spotify_playlist_tracker.Worker.Models.Storage
{
    public class TrackEntity : TableEntity
    {
        public string Name { get; set; }
        public List<string> Artists { get; set; }
        public int Popularity { get; set; }
        public int TrackLength { get; set; }
        public string Album { get; set; }
        public string ReleaseDate { get; set; }
        public DateTime RetrievedTimestamp { get; set; }

        public int? AlbumArtHeight { get; set; }
        public int? AlbumArtWidth { get; set; }
        public string AlbumArtUrl { get; set; }

    }
}
