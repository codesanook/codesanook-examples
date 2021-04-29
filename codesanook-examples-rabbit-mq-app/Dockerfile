FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers 
COPY *.csproj ./
RUN dotnet restore

#	Copy everything else and build 
COPY . ./
RUN dotnet publish --configuration Release --output out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# ENTRYPOINT ["dotnet", "Codesanook.Examples.RabbitMQApp.dll"]
ENTRYPOINT ["/bin/bash", "-c", "sleep 15 && ls && dotnet Codesanook.Examples.RabbitMQApp.dll"]
