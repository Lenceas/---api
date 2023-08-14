FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8079

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MorningStar.Api/MorningStar.Api.csproj", "MorningStar.Api/"]
COPY ["MorningStar.Extension/MorningStar.Extension.csproj", "MorningStar.Extension/"]
COPY ["MorningStar.Service/MorningStar.Service.csproj", "MorningStar.Service/"]
COPY ["MorningStar.Repository/MorningStar.Repository.csproj", "MorningStar.Repository/"]
COPY ["MorningStar.Model/MorningStar.Model.csproj", "MorningStar.Model/"]
COPY ["MorningStar.Common/MorningStar.Common.csproj", "MorningStar.Common/"]
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