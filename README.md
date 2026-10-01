# Módulo de Créditos - OPA SAS (Backend)

Este es el backend para el módulo de administración de solicitudes de crédito de asociados, desarrollado como parte de la Prueba Técnica para OPA SAS.

## Tecnologías Principales
*   **.NET 9** (C# 13)
*   **PostgreSQL** como motor de persistencia.
*   **Entity Framework Core** (Code-First).
*   **Arquitectura Limpia** (Domain, Application, Infrastructure, Api).
*   **Patrón de Servicios & Repositorios** (Separación de responsabilidades).
*   **FluentValidation** (Validaciones de reglas de negocio).
*   **Scalar / OpenAPI** (Documentación de API).
*   **xUnit & Moq** (Pruebas unitarias).

## Requisitos Previos
*   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [Docker Desktop](https://www.docker.com/products/docker-desktop/) (opcional, para levantar la base de datos fácilmente).
*   Una instancia de PostgreSQL en ejecución.

## Configuración y Ejecución Local

1. **Configurar Base de Datos:**
   Si tienes Docker instalado, puedes iniciar la base de datos utilizando el archivo `docker-compose.yml` provisto:
   ```bash
   docker-compose up -d db
   ```
   *Esto levantará un PostgreSQL en el puerto 5432.*

2. **Configurar Variables de Entorno (`appsettings.json`):**
   Dirígete a la carpeta `Opa.Credits.Api` y asegúrate de que tu `appsettings.Development.json` o `appsettings.json` tenga la cadena de conexión correcta (puedes guiarte con `appsettings.example.json`):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=OPA;Username=admindb;Password=TuPassword"
   }
   ```

3. **Aplicar Migraciones (Crear Esquema):**
   Abre una terminal en la raíz del proyecto y ejecuta:
   ```bash
   dotnet ef database update --project Opa.Credits.Infrastructure --startup-project Opa.Credits.Api
   ```
   *(Alternativamente, se incluye un script `database_schema.sql` en la raíz del proyecto que puedes ejecutar manualmente en tu gestor de DB).*

4. **Ejecutar la API:**
   ```bash
   dotnet run --project Opa.Credits.Api
   ```
   La API se ejecutará (usualmente en `http://localhost:5xxx` o `https://localhost:7xxx`).

5. **Documentación de la API (OpenAPI):**
   Navega a la ruta `/scalar` en tu navegador (por ejemplo `https://localhost:7290/scalar/v1`) para explorar e interactuar con los endpoints utilizando la interfaz de **Scalar** (alternativa moderna a Swagger).

## Pruebas Automatizadas
Para ejecutar la suite de pruebas unitarias (validación de reglas de negocio, transiciones inválidas, creación y cálculo de webhook):
```bash
dotnet test
```

## Documentación Adicional
Para conocer a fondo las decisiones técnicas, manejo de seguridad, justificación de arquitectura y respuestas sobre escalabilidad, por favor lee el documento adjunto:
👉 [**DOCUMENTACION.md**](./DOCUMENTACION.md)
