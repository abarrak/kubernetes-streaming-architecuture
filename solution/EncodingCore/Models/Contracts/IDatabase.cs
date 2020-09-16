using System;
using Microsoft.Data.Sqlite;

namespace EncodingCore.Models.Contracts
{
    public interface IDatabase
    {
        SqliteConnection GetConnection();
    }
}
