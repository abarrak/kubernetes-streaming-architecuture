#!/bin/bash

sqlite3 database.db 'CREATE TABLE IF NOT EXISTS MediaFile (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Name VARCHAR(100) NOT NULL,
  Type VARCHAR(50) NOT NULL,
  Size DECIMAL(10, 2),
  FilePath TEXT NOT NULL,
  ManifestPath TEXT NOT NULL,
  Description TEXT,
  UploadedAt DATETIME
);' 'ALTER TABLE MediaFile ADD LockForEncoding INTEGER DEFAULT 0;' '.exit'
