# Base image para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Base image para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MedicalScheduling/MedicalScheduling.csproj", "MedicalScheduling/"]
RUN dotnet restore "MedicalScheduling/MedicalScheduling.csproj"
COPY . .
WORKDIR "/src/MedicalScheduling"
RUN dotnet build -c Release -o /app/build

# Publica a aplicação
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Configuração final para runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicalScheduling.dll"]
