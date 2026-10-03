# Documentación técnica de Sonora

## Diccionario de datos

| Tabla | Campo | Tipo | Descripción |
|---|---|---|---|
| Users | Id | integer | Identificador del usuario |
| Users | Name / Email | text | Datos de contacto |
| Plans | Id, Name, Price | integer/text/decimal | Plan comercial y precio mensual |
| Songs | Id, Title, Artist, Album, Genre, DurationSeconds | integer/text | Catálogo musical |
| Subscriptions | Id, UserId, PlanId, StartDate, EndDate, Status | integer/datetime/text | Ciclo de vida de una suscripción |
| Playlists | Id, UserId, Name | integer/text | Playlist creada por un usuario |

## Diagrama entidad-relación

```mermaid
erDiagram
  USERS ||--o{ SUBSCRIPTIONS : owns
  PLANS ||--o{ SUBSCRIPTIONS : defines
  USERS ||--o{ PLAYLISTS : creates
  USERS { int Id PK string Name string Email }
  PLANS { int Id PK string Name decimal Price string Description }
  SONGS { int Id PK string Title string Artist string Album string Genre int DurationSeconds }
  SUBSCRIPTIONS { int Id PK int UserId FK int PlanId FK datetime StartDate datetime EndDate string Status }
  PLAYLISTS { int Id PK int UserId FK string Name }
```

## Diagrama de clases

```mermaid
classDiagram
  class User
  class Plan
  class Song
  class Subscription
  class Playlist
  User "1" --> "*" Subscription
  Plan "1" --> "*" Subscription
  User "1" --> "*" Playlist
  Playlist "*" --> "*" Song
```

## Diagrama de componentes

```mermaid
flowchart LR
  Browser[React frontend] -->|REST JSON| API[ASP.NET Core API]
  API --> EF[Entity Framework Core]
  EF --> DB[(SQLite)]
  API --> Swagger[OpenAPI / Swagger]
```

## Diagrama de despliegue

```mermaid
flowchart TB
  GH[GitHub Actions] --> Image[GitHub Container Registry]
  GH --> TF[Terraform]
  TF --> Azure[Azure Container Instance]
  Image --> Azure
  User[Usuario] --> Azure
```

## Generación

`generate-documentation.yml` publica este documento como artefacto en cada ejecución manual o cambio en `main`. La infraestructura se valida con Terraform y los análisis de Sonar, Snyk y Semgrep se ejecutan en workflows separados.
