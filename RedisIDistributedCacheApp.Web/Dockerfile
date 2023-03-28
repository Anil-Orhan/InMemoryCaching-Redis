FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RedisIDistributedCacheApp.Web/RedisIDistributedCacheApp.Web.csproj", "RedisIDistributedCacheApp.Web/"]
RUN dotnet restore "RedisIDistributedCacheApp.Web/RedisIDistributedCacheApp.Web.csproj"
COPY . .
WORKDIR "/src/RedisIDistributedCacheApp.Web"
RUN dotnet build "RedisIDistributedCacheApp.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RedisIDistributedCacheApp.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedisIDistributedCacheApp.Web.dll"]
