FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Currency-API.csproj", "./"]
RUN dotnet restore "Currency-API.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Currency-API.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Currency-API.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Currency-API.dll"]
