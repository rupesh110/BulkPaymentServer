FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY backend/BulkPaymentServer.sln .
COPY backend/BulkPaymentServer.Api/BulkPaymentServer.Api.csproj BulkPaymentServer.Api/
COPY backend/BulkPaymentServer.Application/BulkPaymentServer.Application.csproj BulkPaymentServer.Application/
COPY backend/BulkPaymentServer.Domain/BulkPaymentServer.Domain.csproj BulkPaymentServer.Domain/
COPY backend/BulkPaymentServer.Infrastructure/BulkPaymentServer.Infrastructure.csproj BulkPaymentServer.Infrastructure/

RUN dotnet restore BulkPaymentServer.Api/BulkPaymentServer.Api.csproj

COPY backend/. backend/

WORKDIR /src/backend/BulkPaymentServer.Api
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BulkPaymentServer.Api.dll"]