using Microsoft.WindowsAzure.Storage.Table;

namespace spotify_playlist_tracker.Worker.ViewModels
{
    public class PlayedTrackViewModel : TableEntity
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string AlbumArtUrl { get; set; }
        public string AddedBy { get; set; }
        public string AddedByImage { get; set; }
        public int Popularity { get; set; }
        public int Position { get; set; }
        public int TrackLength { get; set; }
    }
}
