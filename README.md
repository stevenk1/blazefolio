# BlazeFolio

## Running with Docker

To run the application with a persistent database, use the provided docker-compose file:

```bash
docker-compose up -d
```

This will start the application and create a persistent volume for the database. The database file will be stored in a Docker volume named `blazefolio_data` and will persist across container restarts and updates.

## Manual Docker Run

Alternatively, you can run the container directly with Docker:

```bash
# Create a volume for the database
docker volume create blazefolio_data

# Run the container
docker run -d \
  --name blazefolio \
  -p 8080:80 \
  -v blazefolio_data:/data \
  -e DatabasePath=/data/blazefolio.db \
  ghcr.io/stevenk1/blazefolio:latest
```

## Accessing the Application

Once running, access the application at http://localhost:8080
