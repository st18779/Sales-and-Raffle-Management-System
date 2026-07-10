# ===========================
# Build Stage
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY server/server.csproj server/

RUN dotnet restore server/server.csproj

COPY . .

WORKDIR /src/server

RUN dotnet publish server.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ===========================
# Runtime Stage
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet","server.dll"]