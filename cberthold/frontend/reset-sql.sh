#!/bin/sh
docker stop sql
docker rm sql
docker run --name sql -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Password123!' -p 1433:1433 -d microsoft/mssql-server-linux:latest