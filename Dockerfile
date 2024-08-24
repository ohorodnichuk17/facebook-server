# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY ["Facebook_Domain/Facebook_Domain.csproj", "Facebook_Domain/"]
COPY ["Facebook_Contracts/Facebook_Contracts.csproj", "Facebook_Contracts/"]
COPY ["Facebook_Infrastructure/Facebook_Infrastructure.csproj", "Facebook_Infrastructure/"]
COPY ["Facebook_Application/Facebook_Application.csproj", "Facebook_Application/"]
COPY ["Facebook_Api/Facebook_Api.csproj", "Facebook_Api/"]
RUN dotnet restore "Facebook_Api/Facebook_Api.csproj"

# copy everything else and build app
COPY . .
WORKDIR /source/Facebook_Api
RUN dotnet publish -o /app


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Facebook_Api.dll"]
