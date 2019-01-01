using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using spotify_playlist_tracker.Worker.Mappers;
using spotify_playlist_tracker.Worker.Services;
using SpotifyWebApi.Auth;
using SpotifyWebApi.Model.Auth;
using SpotifyWebApi.Model.Enum;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace spotify_playlist_tracker.Worker.Controllers
{
    public class CallbackController : Controller
    {
        private readonly ISpotifyAuthService _spotifyAuthService;
        private readonly IStorageService _storageService;

        public CallbackController(ISpotifyAuthService spotifyAuthService, IStorageService storageService)
        {
            _spotifyAuthService = spotifyAuthService;
            _storageService = storageService;
        }

        // GET: /<controller>/
        public IActionResult Index(string code, string state)
        {
            var token = AuthorizationCode.ProcessCallback(_spotifyAuthService.GetAuthParameters(), code);

            //// Use the api with access to personal data.
            _spotifyAuthService.SetToken(token);
            var api = new SpotifyWebApi.SpotifyWebApi(token);
            var me = api.Player.GetCurrentlyPlayingContext();
            var res = me.Result;
            Task.Run(() => FetchTrackAndAdd(_spotifyAuthService, _storageService));
            return View();
        }

        private static void FetchTrackAndAdd(ISpotifyAuthService spotifyAuthService, IStorageService storageService)
        {
            while (true)
            {
                var api = new SpotifyWebApi.SpotifyWebApi(spotifyAuthService.GetToken());
                var currentlyPlayingContext = api.Player.GetCurrentlyPlayingContext().Result;
                var track = CurrentlyPlayingContextToTrackEntity.Map(currentlyPlayingContext);
                storageService.AddTrack(track);

                Thread.Sleep(1000 * 60);
            }
        }
    }
}
