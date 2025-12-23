FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY backend/tMoney.WebApi/tMoney.WebApi.csproj backend/tMoney.WebApi/
COPY backend/tMoney.UnitTests/tMoney.UnitTests.csproj backend/tMoney.UnitTests/
COPY backend/tMoney.Infrastructure/tMoney.Infrastructure.csproj backend/tMoney.Infrastructure/
COPY backend/tMoney.Domain/tMoney.Domain.csproj backend/tMoney.Domain/
COPY backend/tMoney.Application/tMoney.Application.csproj backend/tMoney.Application/
RUN dotnet restore backend/tMoney.WebApi/tMoney.WebApi.csproj

COPY . .

WORKDIR /src/backend/tMoney.WebApi
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT [ "dotnet", "tMoney.WebApi.dll" ]