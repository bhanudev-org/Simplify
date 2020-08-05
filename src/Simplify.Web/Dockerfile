ARG BUILD__VERSION=0.1.0
ARG BACKEND_PROJECT_NAME="Simplify.Web"

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS backend
ARG BUILD__VERSION
ARG BACKEND_PROJECT_NAME
WORKDIR /code
COPY / .
RUN pwd && ls -lrt
WORKDIR /code/src/$BACKEND_PROJECT_NAME
RUN pwd && ls -lrt
RUN dotnet build $BACKEND_PROJECT_NAME.csproj -c Release -o /app/build
RUN dotnet publish $BACKEND_PROJECT_NAME.csproj -c Release -o /app/publish -p:version=$BUILD__VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:5.0
WORKDIR /app
COPY --from=backend /app/publish .
EXPOSE 80/tcp 443/tcp
ENTRYPOINT ["dotnet", "Simplify.Web.dll"]