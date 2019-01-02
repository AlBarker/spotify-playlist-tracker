using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spotify_playlist_tracker.Worker.Models
{
    public class PlaylistTrack
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string AddedBy { get; set; }
    }
}
