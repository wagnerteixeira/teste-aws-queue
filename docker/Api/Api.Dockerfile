# See https://aka.ms/containerfastmode to understand how Visual Studio uses this
# Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY ./src/**/*.csproj ./src/

RUN for file in src/*.csproj; do filename=$(basename -s .csproj $file); filepath="src/$filename"; mkdir $filepath; mv $file $filepath; done

COPY ./src ./src

WORKDIR "/app/src/Api"
RUN dotnet build -c Release -o /app/build

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]