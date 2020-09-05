using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using WebUI.Models.Contracts;
using WebUI.Models.Entities;

namespace WebUI.Models.Data
{
    public class MediaFileRepository : IMediaFileRepository
    {
        private readonly IDatabase _dbConnection;

        public MediaFileRepository(IDatabase dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task Add(MediaFile file)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"
                        INSERT INTO MediaFile
                        (Name, Type, Size, FilePath, MainfestPath, Description, UploadedAt)
                        VALUES(@name, @type, @size, @path, @manifestPath, @desc, @uploadedAt)
                    ";
                    command.Parameters.AddWithValue("@name", file.Name);
                    command.Parameters.AddWithValue("@type", file.Type);
                    command.Parameters.AddWithValue("@size", file.Size);
                    command.Parameters.AddWithValue("@path", file.FilePath);
                    command.Parameters.AddWithValue("@manifestPath", file.FilePath);
                    command.Parameters.AddWithValue("@desc", file.Description);
                    command.Parameters.AddWithValue("@uploadedAt", file.UploadedAt);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<MediaFile> Find(long id)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM MediaFile WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        return MapRecord(reader);
                    }
                    return null;
                }
            }
        }

        public async Task<IEnumerable<MediaFile>> FindAll()
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM MediaFile";

                    var records = new List<MediaFile>();
                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        records.Add(MapRecord(reader));
                    }

                    return records;
                }
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
