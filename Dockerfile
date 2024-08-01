FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.14 AS base
LABEL apiAplication=builder
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.14 AS build
WORKDIR /src
COPY CRub.Api/*.csproj ./CRub.Api/
COPY CRub.Common/*.csproj ./CRub.Common/
COPY CRub.Dal/*.csproj ./CRub.Dal/
COPY CRub.Domain/*.csproj ./CRub.Domain/
COPY CRub.Service/*.csproj ./CRub.Service/

RUN dotnet restore ./CRub.Api/CRub.Api.csproj
COPY . .
WORKDIR "/src/."
RUN dotnet build ./CRub.Api/CRub.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish ./CRub.Api/CRub.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN apk add --no-cache icu-libs
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRub.Api.dll"]
