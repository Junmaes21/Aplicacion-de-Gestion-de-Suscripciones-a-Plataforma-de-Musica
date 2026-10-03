# Sonora — Gestión de suscripciones musicales

Aplicación web full-stack para explorar un catálogo musical, contratar planes y administrar playlists. La API está construida con ASP.NET Core 8 y EF Core; la interfaz usa React + Vite.

## Ejecución local

```bash
docker compose up --build
```

- Frontend: http://localhost:5173
- API: http://localhost:5080
- Salud: http://localhost:5080/health
- Swagger (entorno Development): http://localhost:5080/swagger

Sin Docker, se puede ejecutar `dotnet run --project backend/MusicPlatform.Api` y, en otra terminal, `npm install && npm run dev` dentro de `frontend/`.

## API

| Método | Ruta | Uso |
|---|---|---|
| GET | `/music?search=` | Buscar catálogo |
| GET | `/music/{id}` | Detalle de canción |
| GET | `/plans` | Planes disponibles |
| POST | `/subscriptions` | Crear suscripción (`userId`, `planId`) |
| GET | `/subscriptions/{userId}` | Suscripción activa |
| DELETE | `/subscriptions/{id}` | Cancelar |
| POST | `/playlists` | Crear playlist |
| GET | `/playlists/{userId}` | Playlists del usuario |
| POST | `/reports` | Reporte resumido |

## Calidad y entrega

- [Dockerfile](./Dockerfile) contiene la imagen reproducible del backend.
- [Terraform](./terraform/) aprovisiona Azure Container Instances.
- [Documentación técnica y diagramas](./docs/architecture.md).
- Workflows en [.github/workflows](./.github/workflows): CI, SonarQube, Snyk/Semgrep, infraestructura, despliegue y documentación.
- Las credenciales se configuran como secretos `SONAR_TOKEN`, `SONAR_HOST_URL` y `SNYK_TOKEN` en GitHub; nunca se guardan en el código.

## Entrega final

- URL de la aplicación publicada: `https://<configurar-dominio>`
- URL del repositorio: `https://github.com/Junmaes21/Aplicacion-de-Gestion-de-Suscripciones-a-Plataforma-de-Musica`
- URL de SonarQube: `https://<configurar-sonar-project>`
