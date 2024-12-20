# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project file(s) and restore dependencies
COPY *.sln .
COPY chatservice/*.csproj ./chatservice/
RUN dotnet restore

# Copy the entire project and build it
COPY chatservice/ ./chatservice/
WORKDIR /app/chatservice
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/chatservice/out ./

# Expose the API ports
EXPOSE 5000
EXPOSE 5001

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "chatservice.dll"]
