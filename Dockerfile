
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://*:${PORT}


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Assert.API/Assert.API.csproj", "Assert.API/"]
COPY ["Assert.Application/Assert.Application.csproj", "Assert.Application/"]
COPY ["Assert.Domain/Assert.Domain.csproj", "Assert.Domain/"]
COPY ["Assert.Infrastructure/Assert.Infrastructure.csproj", "Assert.Infrastructure/"]
RUN dotnet restore "Assert.API/Assert.API.csproj"

COPY . .
WORKDIR "/src/Assert.API"
RUN dotnet build "Assert.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Assert.API.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Assert.API.dll"]