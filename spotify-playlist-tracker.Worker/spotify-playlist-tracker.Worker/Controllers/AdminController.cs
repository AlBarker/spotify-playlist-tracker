using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using spotify_playlist_tracker.Worker.Mappers;
using spotify_playlist_tracker.Worker.Models;
using spotify_playlist_tracker.Worker.Services;
using spotify_playlist_tracker.Worker.ViewModels;
using SpotifyWebApi.Auth;
using SpotifyWebApi.Model.Uri;

namespace spotify_playlist_tracker.Worker.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISpotifyAuthService _spotifyAuthService;
        private readonly IStorageService _storageService;
        private Dictionary<string, string> _usernames;

        public AdminController(ISpotifyAuthService spotifyAuthService, IStorageService storageService)
        {
            _spotifyAuthService = spotifyAuthService;
            _storageService = storageService;
            _usernames = new Dictionary<string, string>
            {
                { "karnage11i", "Alex Karney" },
                { "magsatire", "Jack McGrath" },
                { "1232101260", "Chris Quigley" },
                { "1238290776", "Joshua Landy" },
                { "1233033915", "Alex Barker" },
                { "1244598275", "Dan Hornblower" },
                { "0o061owch3c4i23oxchbq5tll", "Josh Anderson" }
            };
        }

        public IActionResult Index()
        {
            ViewBag.HasToken = _spotifyAuthService.GetToken() != null;
            ViewBag.IsListening = _spotifyAuthService.IsListening;
            return View();
        }

        public IActionResult StartListening()
        {
            Task.Run(() => FetchTrackAndAdd(_spotifyAuthService, _storageService));
            _spotifyAuthService.IsListening = true;
            return RedirectToAction("index");
        }

        public IActionResult StopListening()
        {
            _spotifyAuthService.IsListening = false;
            return RedirectToAction("index");
        }

        public IActionResult ClearTracks()
        {
            _storageService.ClearAllTracks();
            return RedirectToAction("index");
        }

        public IActionResult FetchToken()
        {
            var url = AuthorizationCode.GetUrl(_spotifyAuthService.GetAuthParameters(), "");

            return Redirect(url);
        }

        public async Task<IActionResult> SavePlayedTracksAsync()
        {
            var playlistTracks = new List<PlaylistTrack>();

            if (_spotifyAuthService.GetToken() != null)
            {
                var api = new SpotifyWebApi.SpotifyWebApi(_spotifyAuthService.GetToken());
                var fullPlaylistTracks = api.Playlist.GetPlaylistTracks(SpotifyUri.Make("1233033915", "3bdydssu6hXzOP4kLrI8cL")).Result;

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

            await _storageService.AddPlayedTracksAsync(playedTrackViewModels);

            return RedirectToAction("index");

        }

        private static void FetchTrackAndAdd(ISpotifyAuthService spotifyAuthService, IStorageService storageService)
        {
            while (true && spotifyAuthService.IsListening)
            {
                var api = new SpotifyWebApi.SpotifyWebApi(spotifyAuthService.GetToken());
                var currentlyPlayingContext = api.Player.GetCurrentlyPlayingContext().Result;
                if (currentlyPlayingContext != null)
                {
                    var track = CurrentlyPlayingContextToTrackEntity.Map(currentlyPlayingContext);
                    storageService.AddTrack(track);

                    Thread.Sleep(currentlyPlayingContext.Item.DurationMs - currentlyPlayingContext.ProgressMs.Value + 2000);
                }
                else
                {
                    Thread.Sleep(30 * 1000);
                }
            }
        }
    }
}
