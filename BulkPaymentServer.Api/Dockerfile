FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY BulkPaymentServer.sln .
COPY BulkPaymentServer.Api/BulkPaymentServer.Api.csproj BulkPaymentServer.Api/
COPY BulkPaymentServer.Application/BulkPaymentServer.Application.csproj BulkPaymentServer.Application/
COPY BulkPaymentServer.Domain/BulkPaymentServer.Domain.csproj BulkPaymentServer.Domain/
COPY BulkPaymentServer.Infrastructure/BulkPaymentServer.Infrastructure.csproj BulkPaymentServer.Infrastructure/

RUN dotnet restore BulkPaymentServer.Api/BulkPaymentServer.Api.csproj

COPY . .
WORKDIR /src/BulkPaymentServer.Api
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BulkPaymentServer.Api.dll"]