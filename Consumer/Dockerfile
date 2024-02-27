# Base image
FROM mcr.microsoft.com/dotnet/sdk AS build
WORKDIR /app

# Copy and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the files and build the application
COPY . ./
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Install netcat for the wait script
RUN apt-get update && apt-get install -y netcat

# Copy the wait-for-rabbitmq.sh script into the Docker image
COPY wait_for_rabbitmq.sh /wait_for_rabbitmq.sh
RUN chmod +x /wait_for_rabbitmq.sh

# Set the entry point for the container to the wait script
ENTRYPOINT ["/wait_for_rabbitmq.sh", "dotnet", "Consumer.dll"]
