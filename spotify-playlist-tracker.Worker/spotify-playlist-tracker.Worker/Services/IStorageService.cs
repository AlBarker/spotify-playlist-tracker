using spotify_playlist_tracker.Worker.Models.Storage;
using spotify_playlist_tracker.Worker.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace spotify_playlist_tracker.Worker.Services
{
    public interface IStorageService
    {
        void AddTrack(TrackEntity track);
        void ClearAllTracks();
        Task<List<TrackEntity>> GetPlayedTracks();
        Task AddPlayedTracksAsync(List<PlayedTrackViewModel> playedTrackViewModels);
    }
}
