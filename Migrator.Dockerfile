FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine3.14 AS base
LABEL migratorAplication=builder
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.14 AS build
WORKDIR /src
COPY JRub.Migrator/*.csproj ./JRub.Migrator/
COPY JRub.Dal/*.csproj ./JRub.Dal/
COPY JRub.Domain/*.csproj ./JRub.Domain/

RUN dotnet restore ./JRub.Migrator/JRub.Migrator.csproj
COPY . .
WORKDIR "/src/."
RUN dotnet build ./JRub.Migrator/JRub.Migrator.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish ./JRub.Migrator/JRub.Migrator.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JRub.Migrator.dll"]
