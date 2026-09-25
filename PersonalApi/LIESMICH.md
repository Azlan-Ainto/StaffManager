# Personal API (Projekt 3)

## Projektbeschreibung
Entwicklung einer ASP.NET Core REST-API zur Personalverwaltung.

## Lernfortschritt Tag 13

- Neues ASP.NET Core Web API Projekt aufgesetzt und Standard-Templates aufgeräumt.
- REST-Prinzipien und Controller-Architektur kennengelernt.
- HTTP GET (`[HttpGet]`) und HTTP POST (`[HttpPost]`) Endpunkte im `PersonalController` implementiert.
- Statuscodes (200 OK, 201 Created, 404 Not Found) angewendet.
- API interaktiv über die integrierte Swagger-UI getestet.


## Lernfortschritt Tag 14
- Entity Framework Core in ASP.NET Core Web API integriert.
- `appsettings.json` für die sichere Auslagerung der SQL-Verbindungszeichenfolge verwendet.
- `PersonalKontext` als Service im Dependency Injection (DI) Container registriert (`AddDbContext`).
- Controller auf asynchrone EF Core Zugriffe (`ToListAsync`, `SaveChangesAsync`) refaktorisiert.
- Persistenz der API durch SQL Server LocalDB sichergestellt.

## Lernfortschritt Tag 15
- REST-Schnittstelle komplettiert: HTTP PUT (Aktualisieren) und HTTP DELETE (Löschen) implementiert.
- Datenübertragungsobjekte (DTOs) eingeführt, um Entitäten von der API-Oberfläche zu entkoppeln (Over-Posting-Schutz).
- Manuelles Objekt-Mapping zwischen Entitäten und DTOs umgesetzt.
- HTTP-Statuscodes `204 No Content` und `400 Bad Request` in der Praxis angewandt.


## Lernfortschritt Tag 16
- AutoMapper via NuGet installiert und in den DI-Container integriert (`AddAutoMapper`).
- Mapping-Profile (`PersonalProfil`) definiert, inkl. komplexer Eigenschaften-Zuweisung (`ForMember`).
- Controller Refactoring: Manuelles Objekt-Mapping durch sauberes AutoMapper-Mapping ersetzt.
- Konzept der globalen Fehler-Middleware (`UseDeveloperExceptionPage`) verstanden.

## Lernfortschritt Tag 18
- Service-Schicht (`PersonalService`) für komplexe Geschäftslogik in die Clean Architecture eingefügt.
- Domänenspezifische Validierung (Altersprüfung) im Service implementiert.
- Ausnahmen (`ArgumentException`) im Controller sauber abgefangen und als `400 Bad Request` an den Client zurückgegeben.
- Code-First-Migration (`GeburtsdatumHinzugefuegt`) durchgeführt, um das SQL-Schema zu aktualisieren.

## Lernfortschritt Tag 19
- HTTP GET Endpunkt für komplexe Suchanfragen (`/api/personal/suche`) erstellt.
- Query Strings (Abfrageparameter) mittels `[FromQuery]` im Controller entgegengenommen.
- Dynamische SQL-Abfragen mit `IQueryable<T>` im Repository implementiert.
- Verzögerte Ausführung (Deferred Execution) genutzt, um Out-of-Memory-Probleme bei großen Datenmengen zu verhindern.
- SQL-Übersetzung im `DebugView` von EF Core analysiert.


## Lernfortschritt Tag 21
- Web-API mit JSON Web Tokens (JWT) gegen unbefugten Zugriff abgesichert.
- `AuthCtlr` zur Erzeugung digital signierter Tokens implementiert.
- `[Authorize]`-Attribut zum Sperren von API-Endpunkten genutzt.
- Swagger-UI konfiguriert, um `Bearer`-Tokens im HTTP-Header zu übertragen.
- Identität des Aufrufers über `User.Identity` aus dem Token im Controller ausgelesen.