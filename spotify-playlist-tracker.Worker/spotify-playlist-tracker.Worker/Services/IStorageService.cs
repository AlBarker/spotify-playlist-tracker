using spotify_playlist_tracker.Worker.Models.Storage;

namespace spotify_playlist_tracker.Worker.Services
{
    public interface IStorageService
    {
        void AddTrack(TrackEntity track);
        void ClearAllTracks();
    }
}
