#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Parser.WebApi/Parser.WebApi.csproj", "Parser.WebApi/"]
RUN dotnet restore "Parser.WebApi/Parser.WebApi.csproj"
COPY . .
WORKDIR "/src/Parser.WebApi"
RUN dotnet build "Parser.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Parser.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Parser.WebApi.dll"]