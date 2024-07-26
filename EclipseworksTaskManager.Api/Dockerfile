FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY --from=base /app/out .
ENTRYPOINT [ "dotnet", "EclipseworksTaskManager.Api.dll" ]