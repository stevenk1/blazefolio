# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files first for better layer caching
COPY *.sln .
COPY BlazeFolio/*.csproj BlazeFolio/
COPY BlazeFolio.Domain/*.csproj BlazeFolio.Domain/
COPY BlazeFolio.Application/*.csproj BlazeFolio.Application/
COPY BlazeFolio.Infrastructure/*.csproj BlazeFolio.Infrastructure/
COPY BlazeFolio.Application.Tests/*.csproj BlazeFolio.Application.Tests/

# Restore as distinct layers
RUN dotnet restore BlazeFolio.sln

# Copy everything else and build
COPY . .
RUN dotnet build BlazeFolio/BlazeFolio.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish BlazeFolio/BlazeFolio.csproj -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create directory for database that will be mounted as a volume
RUN mkdir -p /data

COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
# Set the database path to the mounted volume location
ENV DatabasePath=/data/blazefolio.db

VOLUME ["/data"]

EXPOSE 80

ENTRYPOINT ["dotnet", "BlazeFolio.dll"]
