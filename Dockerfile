FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ESTMS.API.Host/ESTMS.API.Host.csproj", "src/ESTMS.API.Host/"]
RUN dotnet restore "src/ESTMS.API.Host/ESTMS.API.Host.csproj"
COPY . .
WORKDIR "/src/src/ESTMS.API.Host"
RUN dotnet build "ESTMS.API.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESTMS.API.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESTMS.API.Host.dll"]
