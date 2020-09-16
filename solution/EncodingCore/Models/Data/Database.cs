﻿using System.IO;
using EncodingCore.Models.Contracts;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace EncodingCore.Models.Data
{
    public class Database : IDatabase
    {
        private readonly SqliteConnection _connection;
        private readonly IConfiguration _configuration;

        public Database(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = EstablisConnection();
        }

        public SqliteConnection GetConnection()
        {
            return _connection;
        }

        private SqliteConnection EstablisConnection()
        {
            return new SqliteConnection(_configuration.GetConnectionString("Sqlite"));
        }
    }
}
