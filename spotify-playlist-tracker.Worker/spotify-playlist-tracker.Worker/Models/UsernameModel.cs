namespace spotify_playlist_tracker.Worker.Models
{
    public class UsernameModel
    {
        public UsernameModel (string name, string imageName)
        {
            Name = name;
            ImageName = imageName;
        }
        public string Name { get; set; }
        public string ImageName { get; set; }
    }
}
