# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Install wget for healthchecking
RUN  apt-get update \
    && apt-get install -y wget \
    && rm -rf /var/lib/apt/lists/*

ARG CloudinarySettings__CloudName
ARG CloudinarySettings__ApiKey
ARG CloudinarySettings__ApiSecret

ENV CloudinarySettings__CloudName=$CloudinarySettings__CloudName
ENV CloudinarySettings__ApiKey=$CloudinarySettings__ApiKey
ENV CloudinarySettings__ApiSecret=$CloudinarySettings__ApiSecret

# Copy the database folder to the container
COPY ./SoleMates/Database/ /app/database/

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./SoleMates/SoleMates.csproj ./
RUN dotnet restore "SoleMates.csproj"
COPY . .
WORKDIR "/src/SoleMates"
RUN dotnet build "SoleMates.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "SoleMates.csproj" -c Release -o /app/publish

# Use the runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "SoleMates.dll"]