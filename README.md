# Blockbuster

## Setup
The application will run using docker-compose. The following steps will guide you through the setup process.

### Prerequisites
- Docker
- Docker Compose
- .NET 8.0

### Steps

1. Clone the repository
2. Navigate to the root of the repository
3. Run the following command to start the application
	```sh
	docker-compose up
	```
4. The application will be available at `http://localhost:?????`


### Database

#### Create Database
``sh
dotnet ef migrations add InitialCreate --context BlockbusterDbContext -o Data/BlockbusterMoviesMigrations


In theory, docker compose will create all tables, but soimetimes, the user tables not work, so you can run the following command to create the tables
don't forget to change the connection string in the appsettings.json file to use localhost instead of the database name from docker compose
``sh
dotnet ef database update --context ApplicationDbContext

``sh
dotnet ef migrations script --idempotent --context ApplicationDbContext --output ../sql/application_db_context.sql