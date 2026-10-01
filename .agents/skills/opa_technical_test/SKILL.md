---
name: opa-technical-test
description: Reglas y directrices para el desarrollo de la prueba técnica de OPA SAS (Backend .NET, Postgres, Frontend Angular).
---

# Reglas de Negocio y Tecnológicas (Prueba Técnica OPA SAS)

## Stack Tecnológico Obligatorio
- **Backend:** .NET (C#)
- **Base de datos:** PostgreSQL
- **Frontend:** Angular

## Arquitectura y Patrones (Backend)
- **Clean Architecture:** El proyecto DEBE dividirse en capas claras para mantener el desacoplamiento:
  - **Domain:** Entidades de negocio (`Credito`, `Asociado`), enums de estados, y excepciones de dominio. NO debe tener dependencias externas.
  - **Application:** Casos de uso (Commands/Queries), DTOs, validaciones de negocio (ej. FluentValidation), e interfaces de repositorios.
  - **Infrastructure:** Implementación de acceso a datos (`DbContext` de EF Core con Npgsql), configuración de base de datos, implementaciones del `Repository`, e integración con Webhooks (HttpClient).
  - **API:** Controladores REST, inyección de dependencias, middlewares (ej. manejo global de excepciones).
- **Patrón Repository:** Obligatorio para abstraer el acceso a datos.
- **Principios SOLID:** Estricta aplicación (Responsabilidad Única, Inversión de Dependencias, etc.).

## Manejo de Errores Consistente
La API debe utilizar códigos HTTP apropiados (200, 201, 400, 404, 409, 422, 500). Toda respuesta de error debe tener estrictamente el formato especificado en el PDF:
```json
{
  "success": false,
  "error": {
    "code": "CODIGO_DE_ERROR",
    "message": "Mensaje legible"
  }
}
```

## Pruebas Automatizadas
- Desarrollar **Pruebas Unitarias** cubriendo: creación de crédito, validaciones, consulta inexistente, cambio de estado, transiciones inválidas y validación de la lógica de webhook.

## Seguridad de la API
Para asegurar una API financiera de este tipo, se implementarán los siguientes mecanismos:
1. **Autenticación y Autorización JWT (JSON Web Tokens):** Para verificar la identidad de quien consume el endpoint y proteger las rutas.
2. **CORS Restrictivo:** Configurar orígenes permitidos explícitamente hacia el cliente Angular.
3. **Rate Limiting:** Prevenir ataques de denegación de servicio (DDoS) o fuerza bruta, clave para APIs expuestas.
4. **Validación de Datos (Sanitización):** Uso de `FluentValidation` para asegurar que no entren datos anómalos o inyecciones maliciosas.

## Concurrencia y Alta Escalabilidad (1000 Req/sec)
1. **Manejo de Concurrencia de Datos:** Para manejar intentos simultáneos de modificación sobre el mismo crédito, se utilizará **Concurrencia Optimista** (utilizando el feature de `xmin` de PostgreSQL o un `RowVersion` explícito en EF Core). Esto lanzará una excepción `DbUpdateConcurrencyException` si dos usuarios modifican el crédito al mismo tiempo, evitando inconsistencias financieras. Para procesos sumamente críticos, se puede usar bloqueo pesimista (Row-level locking).
2. **Arquitectura Asíncrona:** Toda la cadena (Controlador -> Aplicación -> Repositorio) debe ser `async`/`await` para no bloquear los hilos (ThreadPool) de .NET.
3. **Webhooks No Bloqueantes:** La llamada al Webhook externo **no debe** pausar el Response del usuario. Se enviará a través de un canal asíncrono (`Channel<T>` o un `BackgroundService` en .NET).
4. **Pool de Conexiones:** Optimización y reutilización de conexiones mediante `Npgsql`.
5. **Lecturas Optimizadas:** Uso de `.AsNoTracking()` en Entity Framework para todas las consultas GET que no requieran actualización.

## Reglas de Negocio Clave
- `valorSolicitado` > 0.
- `tasaInteres` >= 0.
- `numeroCuotas` > 0.
- Control de créditos duplicados.
- Tipos de datos financieros precisos: `DECIMAL(18,2)`.
- Control estricto de transiciones de estado (ej. un crédito NO puede pasar de RECHAZADO a DESEMBOLSADO).
- Trazabilidad y auditoría de cambios.
