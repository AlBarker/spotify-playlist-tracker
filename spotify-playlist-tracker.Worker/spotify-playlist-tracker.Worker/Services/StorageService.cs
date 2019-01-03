using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using spotify_playlist_tracker.Worker.Infrastructure.Settings;
using spotify_playlist_tracker.Worker.Models.Storage;

namespace spotify_playlist_tracker.Worker.Services
{
    public class StorageService : IStorageService
    {
        private readonly IOptions<SettingsModel> _settings;

        public StorageService(IOptions<SettingsModel> settings)
        {
            _settings = settings;
        }
        public async void AddTrack(TrackEntity track)
        {
            CreateTracksTableIfNotExists();

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(track);

            var tracksTable = GetTracksCloudTable();
            // TODO Fix exception here on duplicate
            // Execute the insert operation.
            await tracksTable.ExecuteAsync(insertOperation);

        }

        public async void ClearAllTracks()
        {
            var tracksTable = GetTracksCloudTable();

            await tracksTable.DeleteIfExistsAsync();
        }


        public async Task<List<TrackEntity>> GetPlayedTracks()
        {
            var tracksTable = GetTracksCloudTable();

            TableContinuationToken token = null;
            var entities = new List<TrackEntity>();
            do
            {
                var queryResult = await tracksTable.ExecuteQuerySegmentedAsync(new TableQuery<TrackEntity>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities.OrderBy(x => x.Timestamp).ToList();
        }

        private CloudTable GetTracksCloudTable()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_settings.Value.StorageConnectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
           return  tableClient.GetTableReference("tracks");
        }

        private async void CreateTracksTableIfNotExists()
        {
            var tracksTable = GetTracksCloudTable();

            // Create the table if it doesn't exist.
            await tracksTable.CreateIfNotExistsAsync();
        }
    }
}
