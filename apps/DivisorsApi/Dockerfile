# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ../../DivisorsChallenge.sln ./
COPY ../../Directory.Build.props ./
COPY ../../src/NumberTools/ ./src/NumberTools/
COPY ../../apps/DivisorsApi/ ./apps/DivisorsApi/
RUN dotnet restore ./apps/DivisorsApi/DivisorsApi.csproj
RUN dotnet publish ./apps/DivisorsApi/DivisorsApi.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "DivisorsApi.dll"]
