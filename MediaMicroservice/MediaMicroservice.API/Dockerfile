#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MediaMicroservice.API/MediaMicroservice.API.csproj", "MediaMicroservice.API/"]
COPY ["MediaMicroservice.EntityFramework/MediaMicroservice.EntityFramework.csproj", "MediaMicroservice.EntityFramework/"]
COPY ["MediaMicroservice.Domain/MediaMicroservice.Domain.csproj", "MediaMicroservice.Domain/"]
RUN dotnet restore "MediaMicroservice.API/MediaMicroservice.API.csproj"
COPY . .
WORKDIR "/src/MediaMicroservice.API"
RUN dotnet build "MediaMicroservice.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MediaMicroservice.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MediaMicroservice.API.dll"]