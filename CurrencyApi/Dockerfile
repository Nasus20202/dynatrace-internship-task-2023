FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CurrencyApi.csproj", "./"]
RUN dotnet restore "CurrencyApi.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "CurrencyApi.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "CurrencyApi.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyApi.dll"]
