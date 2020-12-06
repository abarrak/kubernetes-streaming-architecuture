#!/bin/sh

sqlite3 database.db 'CREATE TABLE IF NOT EXISTS MediaFile (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Name VARCHAR(100) NOT NULL,
  Type VARCHAR(50) NOT NULL,
  Size DECIMAL(10, 2),
  FilePath TEXT NOT NULL,
  ManifestPath TEXT NOT NULL,
  Description TEXT,
  LockForEncoding INTEGER DEFAULT 0,
  UploadedAt DATETIME
);' '.exit'
