# See https://aka.ms/containerfastmode to understand how Visual Studio uses this
# Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

RUN mkdir -p api/src
RUN mkdir -p data/src
RUN mkdir -p cross_cutting/src

COPY ["api/src/api.csproj", "./api/src"]
COPY  ./api/src ./api/src

COPY ["data/src/data.csproj", "./data/src"]
COPY  ./data/src ./data/src

COPY ["cross_cutting/src/cross_cutting.csproj", "./cross_cutting/src"]
COPY  ./cross_cutting/src ./cross_cutting/src

WORKDIR "/app/api/src"
RUN dotnet build -c Release -o /app/build

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY ./api/run.sh .
ENTRYPOINT ["sh", "run.sh"]