using Microsoft.AspNetCore.Mvc;
using spotify_playlist_tracker.Worker.Models;
using spotify_playlist_tracker.Worker.Services;
using SpotifyWebApi.Model.Uri;
using System.Collections.Generic;
using System.Linq;

namespace spotify_playlist_tracker.Worker.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly ISpotifyAuthService _spotifyAuthService;

        public PlaylistController(ISpotifyAuthService spotifyAuthService)
        {
            _spotifyAuthService = spotifyAuthService;
        }

        public IActionResult Index()
        {
            var api = new SpotifyWebApi.SpotifyWebApi(_spotifyAuthService.GetToken());
            var fullPlaylistTracks = api.Playlist.GetPlaylistTracks(SpotifyUri.Make("1233033915", "1zjHL1Cxo235n1keiC5IDw")).Result;

            var playlistTracks = new List<PlaylistTrack>();
            foreach (var track in fullPlaylistTracks)
            {
                playlistTracks.Add(new PlaylistTrack
                {
                    Name = track.Track.Name,
                    Artist = track.Track.Artists.FirstOrDefault().Name,
                    AddedBy = track.AddedBy.Id
                });
            }

            List<TopArtist> top5Artists = playlistTracks
                                        .GroupBy(q => q.AddedBy)
                                        .OrderByDescending(gp => gp.Count())
                                        //.Take(5)
                                        .Select(g => new TopArtist { Artist = g.Key, Count = g.Count() }).ToList();

            return View();
        }

        
    }
    public class TopArtist
    {
        public string Artist { get; set; }
        public int Count { get; set; }
    }
}