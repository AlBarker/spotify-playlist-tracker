using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using spotify_playlist_tracker.Worker.Models;
using SpotifyWebApi.Auth;
using SpotifyWebApi.Model.Enum;

namespace spotify_playlist_tracker.Worker.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var state = Guid.NewGuid().ToString(); // Save this state because you must check it later

            var parameters = new AuthParameters
            {
                ClientId = "",
                ClientSecret = "",
                RedirectUri = "https://localhost:44362/callback",
                Scopes = Scope.All,
                ShowDialog = true
            };

            var url = AuthorizationCode.GetUrl(parameters, state);

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
