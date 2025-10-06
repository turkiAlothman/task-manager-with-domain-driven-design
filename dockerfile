FROM  mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

# Copy the project file and restore any dependencies (use .csproj for the project name)
COPY TaskManager.sln ./

COPY "./Application" '/app/Application'
COPY "./Domain" '/app/Domain'
COPY "./infrastructure" '/app/infrastructure'
COPY "./Presentation" '/app/Presentation'
COPY "./TaskManager" '/app/TaskManager'
COPY "./TaskManager.Tests" '/app/TaskManager.Tests'

RUN dotnet restore "TaskManager.sln"

# Copy the rest of the application code
COPY . .

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image

FROM   mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/out ./

# Expose the port your application will run on
EXPOSE 8080

ENTRYPOINT ["dotnet"]

# Start the application
CMD [ "TaskManager.dll" ]