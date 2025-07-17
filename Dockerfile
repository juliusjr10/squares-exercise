FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app/SquaresApi

# Copy solution and projects preserving relative structure
COPY SquaresApi/SquaresApi.sln ./        # Copy sln to /app/SquaresApi/
COPY SquaresApi/*.csproj ./              # Copy SquaresApi project files
COPY ../SquaresApi.Tests/*.csproj ../SquaresApi.Tests/  # This won't work: see notes below

# Instead, copy entire folders preserving sibling structure:

COPY SquaresApi ./SquaresApi
COPY SquaresApi.Tests ../SquaresApi.Tests

# But since WORKDIR is /app/SquaresApi, copying ../SquaresApi.Tests is tricky

# Alternative: set WORKDIR to /app, copy everything as siblings

# So better approach:

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY SquaresApi ./SquaresApi
COPY SquaresApi.Tests ./SquaresApi.Tests

RUN dotnet restore SquaresApi/SquaresApi.sln

RUN dotnet publish SquaresApi/SquaresApi.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/out ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "SquaresApi.dll"]
