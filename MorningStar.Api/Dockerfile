FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 8079

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["MorningStar.Api/MorningStar.Api.csproj", "MorningStar.Api/"]
RUN dotnet restore "MorningStar.Api/MorningStar.Api.csproj"
COPY . .
WORKDIR "/src/MorningStar.Api"
RUN dotnet build "MorningStar.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MorningStar.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MorningStar.Api.dll"]