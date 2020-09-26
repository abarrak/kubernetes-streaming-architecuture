using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using EncodingCore.Models.Contracts;
using EncodingCore.Models.Entities;
using Microsoft.Extensions.Configuration;
using EncodingCore.Models.Utils;

namespace EncodingCore.Models.Managers
{
    public class VideoEncodingManager : IVideoEncodingManager
    {
        private readonly IDatabase _dbConnection;
        private readonly IConfiguration _configuration;

        public VideoEncodingManager(IDatabase dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
        }

        public async Task Process()
        {
            var files = await FetchPendingFiles();

            if (files?.Count() > 0)
            {
                await LockFilesToProcess(files);
                EncodeFiles(files);

                await UpdateFileEncodingData(files);
            }
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
                    command.CommandText = "SELECT * FROM MediaFile WHERE ManifestPath = 0 AND LockForEncoding = 0";

                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        records.Add(MapRecord(reader));
                    }
                }
                return records;
            }
        }

        private async Task LockFilesToProcess(IEnumerable<MediaFile> files)
        {
            var ids = string.Join(",", files.Select(f => f.Id));
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE MediaFile SET LockForEncoding = 1 WHERE Id IN (@ids)";

                    command.Parameters.AddWithValue("@ids", ids);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private void EncodeFiles(IEnumerable<MediaFile> files)
        {
            var storagePath = _configuration.GetValue<string>("FileStorePath");

            foreach (var file in files)
            {
                var fileFullPath = Path.Combine(
                    storagePath,
                    Path.GetDirectoryName(file.FilePath)
                );
                var fileName = Path.GetFileName(file.FilePath);

                var commands = EncodingCommandTemplate.Replace("<FILENAME>", fileName)
                                                      .Replace("<FILE_FULL_PATH>", fileFullPath)
                                                      .Trim();

                commands.Bash();
            }
        }

        private async Task UpdateFileEncodingData(IEnumerable<MediaFile> files)
        {
            var defaultManifestName = _configuration.GetValue<string>("DefaultManifestName");

            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();

                foreach (var file in files)
                {
                    var manifestPath = Path.Combine(
                        Path.GetDirectoryName(file.FilePath),
                        defaultManifestName
                    );

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            "UPDATE MediaFile SET LockForEncoding = 0, ManifestPath = @manifestPath WHERE Id = @id";

                        command.Parameters.AddWithValue("@manifestPath", manifestPath);
                        command.Parameters.AddWithValue("@id", file.Id);

                        await command.ExecuteNonQueryAsync();
                    }
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
                ManifestPath = reader.GetString("ManifestPath"),
                UploadedAt = reader.GetDateTime("UploadedAt")
            };
        }

        private string EncodingCommandTemplate = @"
            cd <FILE_FULL_PATH>
            mkdir manifest
            ffmpeg -i <FILENAME> -vn -acodec libvorbis -ab 128k -dash 1 manifest/<FILENAME>_audio.webm

            ffmpeg -i <FILENAME> -c:v libvpx-vp9 -keyint_min 150 \
            -g 150 -tile-columns 4 -frame-parallel 1  -f webm -dash 1 \
            -an -vf scale=160:90 -b:v 250k -dash 1 manifest/<FILENAME>_160x90_250k.webm \
            -an -vf scale=320:180 -b:v 500k -dash 1 manifest/<FILENAME>_320x180_500k.webm \
            -an -vf scale=640:360 -b:v 750k -dash 1 manifest/<FILENAME>_640x360_750k.webm \
            -an -vf scale=640:360 -b:v 1000k -dash 1 manifest/<FILENAME>_640x360_1000k.webm \


            ffmpeg \
              -f webm_dash_manifest -i manifest/<FILENAME>_160x90_250k.webm \
              -f webm_dash_manifest -i manifest/<FILENAME>_320x180_500k.webm \
              -f webm_dash_manifest -i manifest/<FILENAME>_640x360_750k.webm \
              -f webm_dash_manifest -i manifest/<FILENAME>_640x360_1000k.webm \
              -f webm_dash_manifest -i manifest/<FILENAME>_audio.webm \
              -c copy \
              -map 0 -map 1 -map 2 -map 3 -map 4\
              -f webm_dash_manifest \
              -adaptation_sets 'id=0,streams=0,1,2,3 id=1,streams=4' \
              manifest/manifest.mpd
            ";
    }
}
