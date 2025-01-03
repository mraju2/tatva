# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and project files
COPY ChatService.sln ./
COPY ChatService.csproj ./

# Restore dependencies
RUN dotnet restore

# Copy the entire project and build it
COPY . ./ 
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Copy additional files into the runtime image
COPY system_prompt_sql.txt /app/system_prompt_sql.txt


# Expose the API ports
EXPOSE 8080

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "ChatService.dll"]
