using Microsoft.WindowsAzure.Storage.Table;

namespace spotify_playlist_tracker.Worker.Models.Storage
{
    public class TrackEntity : TableEntity
    {
        public string Name { get; set; }
        public string Artist { get; set; }
    }
}
