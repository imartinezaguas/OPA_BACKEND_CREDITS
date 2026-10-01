# Dockerfile para la API REST (.NET 9)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar proyectos y restaurar dependencias
COPY ["Opa.Credits.Api/Opa.Credits.Api.csproj", "Opa.Credits.Api/"]
COPY ["Opa.Credits.Application/Opa.Credits.Application.csproj", "Opa.Credits.Application/"]
COPY ["Opa.Credits.Domain/Opa.Credits.Domain.csproj", "Opa.Credits.Domain/"]
COPY ["Opa.Credits.Infrastructure/Opa.Credits.Infrastructure.csproj", "Opa.Credits.Infrastructure/"]
RUN dotnet restore "Opa.Credits.Api/Opa.Credits.Api.csproj"

# Copiar todo el código fuente
COPY . .
WORKDIR "/src/Opa.Credits.Api"

# Construir
RUN dotnet build "Opa.Credits.Api.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "Opa.Credits.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final ligera para ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Opa.Credits.Api.dll"]
