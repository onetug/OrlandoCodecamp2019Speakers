#!/bin/sh
dotnet ef migrations add InitialCreate
dotnet ef database update