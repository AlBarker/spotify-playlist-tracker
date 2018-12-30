using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpotifyWebApi.Auth;
using SpotifyWebApi.Model.Enum;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace spotify_playlist_tracker.Worker.Controllers
{
    public class CallbackController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(string code, string state)
        {

            //var state = Guid.NewGuid().ToString(); // Save this state because you must check it later
            var parameters = new AuthParameters
            {
                ClientId = "",
                ClientSecret = "",
                RedirectUri = "https://localhost:44362/callback",
                Scopes = Scope.All,
                ShowDialog = true
            };

            var token = AuthorizationCode.ProcessCallback(parameters, code);

            //// Use the api with access to personal data.
            var api = new SpotifyWebApi.SpotifyWebApi(token);
            var me = api.Player.GetCurrentlyPlaying();
            var res = me.Result;
            return View();
        }
    }
}
