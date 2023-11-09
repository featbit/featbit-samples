#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y 'newrelic-dotnet-agent' \
&& rm -rf /var/lib/apt/lists/*
# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=2fb3c37c7554db3fa55c2224c4ab7fbaFFFFNRAL \
NEW_RELIC_APP_NAME="FeatBitNewRelicOneIntegrationDocker"

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS newrelic
# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y 'newrelic-dotnet-agent' \
&& rm -rf /var/lib/apt/lists/*
# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=2fb3c37c7554db3fa55c2224c4ab7fbaFFFFNRAL \
NEW_RELIC_APP_NAME="FeatBitNewRelicOneIntegrationDocker"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FeatBitNewRelicOneIntegrationDocker/FeatBitNewRelicOneIntegrationDocker.csproj", "FeatBitNewRelicOneIntegrationDocker/"]
RUN dotnet restore "FeatBitNewRelicOneIntegrationDocker/FeatBitNewRelicOneIntegrationDocker.csproj"
COPY . .
WORKDIR "/src/FeatBitNewRelicOneIntegrationDocker"
RUN dotnet build "FeatBitNewRelicOneIntegrationDocker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FeatBitNewRelicOneIntegrationDocker.csproj" -c Release -o /app/publish /p:UseAppHost=false



FROM newrelic AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeatBitNewRelicOneIntegrationDocker.dll"]