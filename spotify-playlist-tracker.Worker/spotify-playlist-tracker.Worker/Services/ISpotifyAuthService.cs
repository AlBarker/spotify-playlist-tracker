using SpotifyWebApi.Auth;
using SpotifyWebApi.Model.Auth;

namespace spotify_playlist_tracker.Worker.Services
{
    public interface ISpotifyAuthService
    {
        Token GetToken();
        void SetToken(Token token);        
        AuthParameters GetAuthParameters();
        bool IsListening { get; set; }

    }
}
