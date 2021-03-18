#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MessageMicroservice.API/MessageMicroservice.API.csproj", "MessageMicroservice.API/"]
COPY ["MessageMicroservice.Domain/MessageMicroservice.Domain.csproj", "MessageMicroservice.Domain/"]
COPY ["MessageMicroservice.Services/MessageMicroservice.Services.csproj", "MessageMicroservice.Services/"]
RUN dotnet restore "MessageMicroservice.API/MessageMicroservice.API.csproj"
COPY . .
WORKDIR "/src/MessageMicroservice.API"
RUN dotnet build "MessageMicroservice.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MessageMicroservice.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageMicroservice.API.dll"]