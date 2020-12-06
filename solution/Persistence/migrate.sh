#!/bin/sh

sqlite3 database.db '.exit'

sqlite3 database.db < 01_initial_script.sql
sqlite3 database.db < 02_add_locking_column_to_mediafile_table.sql
