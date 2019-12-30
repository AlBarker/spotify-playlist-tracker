using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using spotify_playlist_tracker.Worker.Mappers;
using spotify_playlist_tracker.Worker.Models;
using spotify_playlist_tracker.Worker.Services;
using spotify_playlist_tracker.Worker.ViewModels;
using SpotifyWebApi.Auth;
using SpotifyWebApi.Model.Enum;
using SpotifyWebApi.Model.Uri;

namespace spotify_playlist_tracker.Worker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStorageService _storageService;
        private readonly ISpotifyAuthService _spotifyAuthService;
        private Dictionary<string, string> _usernames;

        public HomeController(IStorageService storageService, ISpotifyAuthService spotifyAuthService)
        {
            _storageService = storageService;
            _spotifyAuthService = spotifyAuthService;
            _usernames = new Dictionary<string, string>
            {
                { "karnage11i", "Alex Karney" },
                { "magsatire", "Jack McGrath" },
                { "1232101260", "Chris Quigley" },
                { "1238290776", "Joshua Landy" },
                { "1233033915", "Alex Barker" },
                { "1244598275", "Dan Hornblower" },
                { "0o061owch3c4i23oxchbq5tll", "Josh Anderson" },
                { "12138108557", "Veashka Rojas" },
                { "gardenbed", "Georgia Robinson" },
                { "billysakalis", "Billy Sakalis" }
            };
        }

        public IActionResult Index()
        {
            var playlistTracks = new List<PlaylistTrack>();

            if (_spotifyAuthService.GetToken() != null)
            {
                var api = new SpotifyWebApi.SpotifyWebApi(_spotifyAuthService.GetToken());
                var fullPlaylistTracks = api.Playlist.GetPlaylistTracks(SpotifyUri.Make("1233033915", "1zjHL1Cxo235n1keiC5IDw")).Result;

                foreach (var track in fullPlaylistTracks)
                {
                    playlistTracks.Add(new PlaylistTrack
                    {
                        Name = track.Track.Name,
                        Artist = track.Track.Artists.FirstOrDefault().Name,
                        AddedBy = _usernames.GetValueOrDefault(track.AddedBy.Id)
                    });
                }
            }

            List<PlayedTrackViewModel> playedTrackViewModels = new List<PlayedTrackViewModel>();
            var playedTrackEntities = _storageService.GetPlayedTracks().Result;

            for (var i = 0; i < playedTrackEntities.Count(); i++)
            {
                playedTrackViewModels.Add(TrackEntityToPlayedTrackViewModel.Map(playedTrackEntities[i], i, playlistTracks));
            }

            ViewBag.PlayedTracks = playedTrackViewModels;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
