using System;
using Microsoft.Data.Sqlite;

namespace StreamingCore.Models.Contracts
{
    public interface IDatabase
    {
        SqliteConnection GetConnection();
    }
}
