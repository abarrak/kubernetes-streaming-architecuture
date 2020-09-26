CREATE TABLE IF NOT EXISTS MediaFile (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Name VARCHAR(100) NOT NULL,
  Type VARCHAR(50) NOT NULL,
  Size DECIMAL(10, 2),
  FilePath TEXT NOT NULL,
  ManifestPath TEXT NOT NULL,
  Description TEXT,
  UploadedAt DATETIME
);

INSERT OR IGNORE INTO MediaFile VALUES (
  1,
  'Sample MP4 File',
  '.mp4',
  26800000,
  '5985d465-4b2c-4c27-966b-96a9a09198db/demo.mp4',
  '5985d465-4b2c-4c27-966b-96a9a09198db/manifest/manifest.mpd',
  'A sample MPEG4 served adaptivily by DASH stream',
  '9/4/2020 10:26:19 PM'
);
