# See https://aka.ms/containerfastmode to understand how Visual Studio uses this
# Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

RUN mkdir -p job/src
RUN mkdir -p data/src
RUN mkdir -p cross_cutting/src
RUN mkdir -p BusinessLogic

COPY ["job/src/job.csproj", "./job/src"]
COPY  ./job/src ./job/src

COPY ["data/src/data.csproj", "./data/src"]
COPY  ./data/src ./data/src

COPY ["cross_cutting/src/cross_cutting.csproj", "./cross_cutting/src"]
COPY  ./cross_cutting/src ./cross_cutting/src

COPY ["BusinessLogic/BusinessLogic.csproj", "./BusinessLogic"]
COPY  ./BusinessLogic ./BusinessLogic

WORKDIR "/app/job/src"
RUN dotnet build -c Release -o /app/build

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY ./job/run.sh .
ENTRYPOINT ["sh", "run.sh"]