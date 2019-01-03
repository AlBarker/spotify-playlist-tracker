using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spotify_playlist_tracker.Worker.ViewModels
{
    public class PlayedTrackViewModel
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string AlbumArtUrl { get; set; }
        public string AddedBy { get; set; }
        public int Popularity { get; set; }
        public int Position { get; set; }
        public int TrackLength { get; set; }
    }
}
