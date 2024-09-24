# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the CSPROJ file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code and build
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official .NET Runtime image for running the application
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out ./

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "Example.RabbitMq.Consumer.dll"]
