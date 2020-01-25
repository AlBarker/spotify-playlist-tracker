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
        private Dictionary<string, UsernameModel> _usernames;

        public HomeController(IStorageService storageService, ISpotifyAuthService spotifyAuthService)
        {
            _storageService = storageService;
            _spotifyAuthService = spotifyAuthService;
            _usernames = new Dictionary<string, UsernameModel>
            {
                { "karnage11i", new UsernameModel("Alex Karney", "ak") },
                { "magsatire", new UsernameModel("Jack McGrath", "jm") },
                { "1232101260", new UsernameModel("Chris Quigley", "cq") },
                { "1238290776", new UsernameModel("Joshua Landy", "jl") },
                { "1233033915", new UsernameModel("Alex Barker", "ab") },
                { "1244598275", new UsernameModel("Dan Hornblower", "dh") },
                { "genjamon1234", new UsernameModel("Josh Anderson", "ja") },
                { "12138108557", new UsernameModel("Veashka Rojas", "vr") },
                { "gardenbed", new UsernameModel("Georgia Robinson", "gr") },
                { "billysakalis", new UsernameModel("Billy Sakalis", "bs") },
                { "braeden.wilson", new UsernameModel("Braeden Wilson", "bw") },
                { "1278556031", new UsernameModel("Matt Knightbridge", "mk") },
                { "griffkyn22", new UsernameModel("Griffyn Heels", "gh") }
            };
        }

        public IActionResult Index()
        {
          var playlistTracks = new List<PlaylistTrack>();

            if (_spotifyAuthService.GetToken() != null)
            {
                var api = new SpotifyWebApi.SpotifyWebApi(_spotifyAuthService.GetToken());
                var fullPlaylistTracks = api.Playlist.GetPlaylistTracks(SpotifyUri.Make("1233033915", "6LJOm2SkbNEsmANfIGhemx")).Result;

                foreach (var track in fullPlaylistTracks)
                {
                    var usernameModel = _usernames.GetValueOrDefault(track.AddedBy.Id);
                    playlistTracks.Add(new PlaylistTrack
                    {
                        Name = track.Track.Name,
                        Artist = track.Track.Artists.FirstOrDefault().Name,
                        AddedBy = usernameModel?.Name,
                        AddedByImage = usernameModel?.ImageName
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
