FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RedisInMemory.Web/RedisInMemory.Web.csproj", "RedisInMemory.Web/"]
RUN dotnet restore "RedisInMemory.Web/RedisInMemory.Web.csproj"
COPY . .
WORKDIR "/src/RedisInMemory.Web"
RUN dotnet build "RedisInMemory.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RedisInMemory.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedisInMemory.Web.dll"]
