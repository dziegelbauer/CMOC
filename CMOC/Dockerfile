FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CMOC/CMOC.csproj", "CMOC/"]
COPY ["CMOC.Data/CMOC.Data.csproj", "CMOC.Data/"]
COPY ["CMOC.Domain/CMOC.Domain.csproj", "CMOC.Domain/"]
COPY ["CMOC.Services/CMOC.Services.csproj", "CMOC.Services/"]
RUN dotnet restore "CMOC/CMOC.csproj"
COPY . .
WORKDIR "/src/CMOC"
RUN dotnet build "CMOC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CMOC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY "CMOC/cmoc.db" .
ENTRYPOINT ["dotnet", "CMOC.dll"]
