# Base image para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Base image para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie apenas o arquivo .csproj inicialmente
COPY ["src/MedicalScheduling.csproj", "./"]
RUN dotnet restore "MedicalScheduling.csproj"

# Copie todo o restante do projeto
COPY src/ ./
RUN dotnet build -c Release -o /app/build

# Publica a aplicação
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Configuração final para runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicalScheduling.dll"]
