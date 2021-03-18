#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["UserMicroservice.API/UserMicroservice.API.csproj", "UserMicroservice.API/"]
COPY ["UserMicroservice.EntityFramework/UserMicroservice.EntityFramework.csproj", "UserMicroservice.EntityFramework/"]
COPY ["UserMicroservice.Domain/UserMicroservice.Domain.csproj", "UserMicroservice.Domain/"]
RUN dotnet restore "UserMicroservice.API/UserMicroservice.API.csproj"
COPY . .
WORKDIR "/src/UserMicroservice.API"
RUN dotnet build "UserMicroservice.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserMicroservice.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserMicroservice.API.dll"]