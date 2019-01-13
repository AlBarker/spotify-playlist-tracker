﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using spotify_playlist_tracker.Worker.Mappers;
using spotify_playlist_tracker.Worker.Services;
using SpotifyWebApi.Auth;

namespace spotify_playlist_tracker.Worker.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISpotifyAuthService _spotifyAuthService;
        private readonly IStorageService _storageService;

        public AdminController(ISpotifyAuthService spotifyAuthService, IStorageService storageService)
        {
            _spotifyAuthService = spotifyAuthService;
            _storageService = storageService;
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
