# Sähköinen huoltokirja / Service manual

<img src="https://raw.githubusercontent.com/Lottuska/service-manual/main/serviceManualSwaggerScreenshot.png?token=GHSAT0AAAAAACEZ77OHCW2FSB5ZXCXWN3KEZFH7P5Q" width="1000px" />

## [FIN] Taustatiedot
Rajapinta laitteiden huoltojen kirjaamiseen. Huoltotoimenpiteet kohidstuvat laitteisiin ja niitä voidaan kategorisoida eri kriittisyysluokkaan. Huoltotehtävällä on tieto huollettavasta laitteesta, sen kirjausajankohdasta, kuvaus huoltotarpeesta, huollon kriittisyysluokka sekä tieto onko huolto tehty vai tekemättä.

## [FIN] Rajapinnan toiminnot
- Hakea laitteita
- Hakea yksittäisen laitteen
- Hakea laitekohtaiset huoltotoehtävät
- Hakea kaikki huoltotehtävät
- Hakea yksittäisen huoltotehtävän
- Lisätä uusi huoltotehtävä
- Muokata valittua huoltotehtävää
- Poistaa valittu huoltotehtävä

## [ENG] Background information
API for recording device maintenance. The maintenance tasks are focused on devices anc can be categorized into different criticality levels. The maintenance task has information about the serviced device, its recording time, a description of the needed maintenance, the criticality level and information about whether the task has been done or not.

## [ENG] API endpoints for
- GET all devices
- GET one device
- GET device-specific maintenance tasks
- GET all maintenance tasks
- GET one maintenance task
- POST/add a new maintenance task
- PUT/edit the selected maintenance task
- DELETE the selected maintenance task

## Instructions for setting up a local development environment
To make this app run in Development mode and be able to inspect its endpoints with Swagger, you must:

1. Clone / fork / download this repository to your local setup.
2. Create a new empty MS SQL Server database for this project.
3. Find **EtteplanMORE.ServiceManual.Web** folder and create a copy of appsettings.json and name it appsettings.Development.json
4. Change your connection string in **appsettings.Development.json** to have your own server and database names and possible other credentials.
```
 "ConnectionStrings": {
 	"DefaultConnection": "Server=SERVERNAME; Database=DATABASENAME; Trusted_Connection=True; TrustServerCertificate=True;"
 }
```
5. Navigate yourself to **root** of the project and run ```dotnet restore``` to download all needed NuGet packages.
6. Navigate yourself to **EtteplanMORE.ServiceManual.Web** and run ```dotnet ef database update``` to run migrations to your database.
   - If everything went right, your database should now have empty tables 'FactoryDevices' and 'MaintenanceTasks'.
7. Inside **EtteplanMORE.ServiceManual.Web** run ```dotnet run --environment "Development"``` to start application.
   - Notice that this seeds the database with 1000 rows of 'FactoryDevice' data and 5 rows of example 'MaintenanceTask' data, but only if the tables are empty!
8. Navigate yourself to [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html) or [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html) or any other suggested port and start exploring the available endpoints! :smile:
