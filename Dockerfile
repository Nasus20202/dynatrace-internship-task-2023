FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["currency-api.csproj", "./"]
RUN dotnet restore "currency-api.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "currency-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "currency-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "currency-api.dll"]
