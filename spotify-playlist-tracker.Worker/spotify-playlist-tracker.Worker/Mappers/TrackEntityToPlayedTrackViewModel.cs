using spotify_playlist_tracker.Worker.Models;
using spotify_playlist_tracker.Worker.Models.Storage;
using spotify_playlist_tracker.Worker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spotify_playlist_tracker.Worker.Mappers
{
    public static class TrackEntityToPlayedTrackViewModel
    {
        public static PlayedTrackViewModel Map(TrackEntity track, int index, List<PlaylistTrack> playlistTracks)
        {
            return new PlayedTrackViewModel
            {
                RowKey = Guid.NewGuid().ToString(),
                PartitionKey = Guid.NewGuid().ToString(),
                Name = track.Name,
                Artist = track.Artist,
                Album = track.Album,
                AlbumArtUrl = track.AlbumArtUrl,
                Popularity = track.Popularity,
                Position = 100 - index,
                TrackLength = track.TrackLength,
                AddedBy = playlistTracks.Find(x => x.Name == track.Name && x.Artist == track.Artist)?.AddedBy
            };
        }
    }
}
