# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["BlazeFolio.sln", "."]
COPY ["BlazeFolio/*.csproj", "BlazeFolio/"]
COPY ["BlazeFolio.Domain/*.csproj", "BlazeFolio.Domain/"]
COPY ["BlazeFolio.Application/*.csproj", "BlazeFolio.Application/"]
COPY ["BlazeFolio.Infrastructure/*.csproj", "BlazeFolio.Infrastructure/"]

RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR "/src/BlazeFolio"
RUN dotnet build "BlazeFolio.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BlazeFolio.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create directory for database
RUN mkdir -p /app/Data

COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80

ENTRYPOINT ["dotnet", "BlazeFolio.dll"]
