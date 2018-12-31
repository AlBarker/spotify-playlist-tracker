using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using spotify_playlist_tracker.Worker.Services;
using SpotifyWebApi.Auth;
using SpotifyWebApi.Model.Enum;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace spotify_playlist_tracker.Worker.Controllers
{
    public class CallbackController : Controller
    {
        private readonly ISpotifyAuthService _spotifyAuthService;

        public CallbackController(ISpotifyAuthService spotifyAuthService)
        {
            _spotifyAuthService = spotifyAuthService;
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
            return View();
        }
    }
}
