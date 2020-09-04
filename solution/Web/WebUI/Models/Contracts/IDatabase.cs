using System;
using Microsoft.Data.Sqlite;

namespace WebUI.Models.Contracts
{
    public interface IDatabase
    {
        SqliteConnection GetConnection();
    }
}
