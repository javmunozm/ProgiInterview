# ---- Restore (cached layer) ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY BidCalculator.sln .
COPY src/Domain/*.csproj           src/Domain/
COPY src/Application/*.csproj      src/Application/
COPY src/Infrastructure/*.csproj   src/Infrastructure/
COPY src/Persistence/*.csproj      src/Persistence/
COPY src/API/*.csproj              src/API/
RUN dotnet restore src/API/API.csproj

# ---- Build ----
COPY src/ src/
RUN dotnet publish src/API/API.csproj -c Release -o /publish --no-restore

# ---- Runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /publish .
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080
ENTRYPOINT ["dotnet", "API.dll"]
