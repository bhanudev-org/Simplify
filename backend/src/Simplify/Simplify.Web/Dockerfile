ARG BUILD__VERSION=0.1.0

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS backend
ARG BUILD__VERSION
WORKDIR /code
COPY / .
WORKDIR /code/src/Simplify/Simplify.Web
RUN dotnet build Simplify.Web.csproj -c Release -o /app/build
RUN dotnet publish Simplify.Web.csproj -c Release -o /app/publish -f net5.0 -p:version=$BUILD__VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:5.0
WORKDIR /app
COPY --from=backend /app/publish .
EXPOSE 80/tcp 443/tcp
ENTRYPOINT ["dotnet", "Simplify.Web.dll"]