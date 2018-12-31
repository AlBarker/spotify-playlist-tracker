using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using spotify_playlist_tracker.Worker.Infrastructure.Settings;
using SpotifyWebApi.Auth;
using SpotifyWebApi.Model.Auth;
using SpotifyWebApi.Model.Enum;

namespace spotify_playlist_tracker.Worker.Services
{
    public class SpotifyAuthService : ISpotifyAuthService
    {
        private readonly IOptions<SettingsModel> _settings;

        public SpotifyAuthService(IOptions<SettingsModel> settings)
        {
            _settings = settings;
        }

        public Token Token {
            get
            {
                if (Token.IsExpired)
                {
                    Token = AuthorizationCode.RefreshToken(GetAuthParameters(), Token);
                }
                return Token;
            }
            set
            {
                Token = value;
            }
        }

        public AuthParameters GetAuthParameters()
        {
            return new AuthParameters
            {
                ClientId = _settings.Value.SpotifyClientId,
                ClientSecret = _settings.Value.SpotifyClientSecret,
                RedirectUri = _settings.Value.LoginCallbackUrl,
                Scopes = Scope.All,
                ShowDialog = true
            };
        }
    }
}
