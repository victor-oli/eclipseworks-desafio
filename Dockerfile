FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

COPY . ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

RUN dotnet tool install --global dotnet-ef --version 7.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=base /app/out .

ENTRYPOINT ["dotnet","EclipseworksTaskManager.Api.dll"]