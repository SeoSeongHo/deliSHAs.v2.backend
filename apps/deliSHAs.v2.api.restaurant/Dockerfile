FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["apps/deliSHAs.v2.api.restaurant/deliSHAs.v2.api.restaurant.csproj", "apps/deliSHAs.v2.api.restaurant/"]
RUN dotnet restore "apps/deliSHAs.v2.api.restaurant/deliSHAs.v2.api.restaurant.csproj"
COPY . .
WORKDIR "/src/apps/deliSHAs.v2.api.restaurant"
RUN dotnet build "deliSHAs.v2.api.restaurant.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "deliSHAs.v2.api.restaurant.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "deliSHAs.v2.api.restaurant.dll"]