namespace spotify_playlist_tracker.Worker.Infrastructure.Settings
{
    public class SettingsModel
    {
        public string StorageConnectionString { get; set; }
        public string LoginCallbackUrl { get; set; }
        public string SpotifyClientId { get; set; }
        public string SpotifyClientSecret { get; set; }
    }
}
