using SpotifyWebApi.Auth;
using SpotifyWebApi.Model.Auth;

namespace spotify_playlist_tracker.Worker.Services
{
    public interface ISpotifyAuthService
    {
        Token Token { get; set; }
        AuthParameters GetAuthParameters();
    }
}
