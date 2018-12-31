using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using spotify_playlist_tracker.Worker.Models;
using spotify_playlist_tracker.Worker.Services;
using SpotifyWebApi.Auth;
using SpotifyWebApi.Model.Enum;

namespace spotify_playlist_tracker.Worker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpotifyAuthService _spotifyAuthService;

        public HomeController(ISpotifyAuthService spotifyAuthService)
        {
            _spotifyAuthService = spotifyAuthService;
        }

        public IActionResult Index()
        {
            var url = AuthorizationCode.GetUrl(_spotifyAuthService.GetAuthParameters(), "");

            return Redirect(url);
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
