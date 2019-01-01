using spotify_playlist_tracker.Worker.Models.Storage;
using SpotifyWebApi.Model;
using System.Linq;

namespace spotify_playlist_tracker.Worker.Mappers
{
    public static class CurrentlyPlayingContextToTrackEntity
    {
        public static TrackEntity Map(CurrentlyPlayingContext currentlyPlayingContext)
        {
            return new TrackEntity()
            {
                RowKey = currentlyPlayingContext.Item.Id,
                PartitionKey = currentlyPlayingContext.Item.Artists.Select(x => x.Name).FirstOrDefault(),
                Name = currentlyPlayingContext.Item.Name,
                Artists = currentlyPlayingContext.Item.Artists.Select(x => x.Name).ToList(),
                Popularity = currentlyPlayingContext.Item.Popularity,
                TrackLength = currentlyPlayingContext.Item.DurationMs,
                Album = currentlyPlayingContext.Item.Album.Name,
                // ReleaseDate = currentlyPlayingContext.Item.Album,
                RetrievedTimestamp = currentlyPlayingContext.Timestamp,
                AlbumArtHeight = currentlyPlayingContext.Item.Album.Images.FirstOrDefault().Height,
                AlbumArtWidth = currentlyPlayingContext.Item.Album.Images.FirstOrDefault().Width,
                AlbumArtUrl = currentlyPlayingContext.Item.Album.Images.FirstOrDefault().Url
            };
        }
    }
}
