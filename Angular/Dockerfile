FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.1-sdk AS build
#setup node
ENV NODE_VERSION 8.11.1
ENV NODE_DOWNLOAD_SHA 0e20787e2eda4cc31336d8327556ebc7417e8ee0a6ba0de96a09b0ec2b841f60
RUN apt-get update && apt-get install curl -y
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs
WORKDIR /src
COPY /Angular.csproj Angular/
RUN dotnet restore Angular/Angular.csproj
COPY . .
RUN dotnet build Angular.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Angular.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Angular.dll"]
