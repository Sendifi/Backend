# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY backSendify/backSendify.csproj backSendify/
RUN dotnet restore backSendify/backSendify.csproj

COPY backSendify/. backSendify/
WORKDIR /src/backSendify
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
COPY scripts/render-entrypoint.sh .

RUN chmod +x /app/render-entrypoint.sh

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["./render-entrypoint.sh"]
