using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using EncodingCore.Models.Contracts;
using EncodingCore.Models.Entities;

namespace EncodingCore.Models.Managers
{
    public class VideoEncodingManager : IVideoEncodingManager
    {
        private readonly IDatabase _dbConnection;

        public VideoEncodingManager(IDatabase dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task Process()
        {
            var files = await FetchPendingFiles();
        }

        private async Task<IEnumerable<MediaFile>> FetchPendingFiles()
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();

                var records = new List<MediaFile>();
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM MediaFile WHERE MainfestPath IS NULL";

                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        records.Add(MapRecord(reader));
                    }
                }
                return records;
            }
        }

        private MediaFile MapRecord(SqliteDataReader reader)
        {
            return new MediaFile
            {
                Id = reader.GetInt64("Id"),
                Name = reader.GetString("Name"),
                Type = reader.GetString("Type"),
                Size = reader.GetInt64("Size"),
                Description = reader.GetString("Description"),
                FilePath = reader.GetString("FilePath"),
                MainfestPath = reader.GetString("MainfestPath"),
                UploadedAt = reader.GetDateTime("UploadedAt")
            };
        }
    }
}
