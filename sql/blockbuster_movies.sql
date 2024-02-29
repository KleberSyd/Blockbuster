-- Create a new database called blockbuster_movies
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'blockbuster_movies')
BEGIN
    CREATE DATABASE blockbuster_movies;
END
GO

-- Select the database
USE blockbuster_movies;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240229201249_InitialCreate'
)
BEGIN
    CREATE TABLE [Movies] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [Genre] nvarchar(max) NOT NULL,
        [Director] nvarchar(max) NOT NULL,
        [Year] int NOT NULL,
        [Rating] int NOT NULL,
        [ImageUrl] nvarchar(max) NOT NULL,
        [TrailerUrl] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [IsAvailable] bit NOT NULL,
        [Timespan] int NOT NULL,
        [CreateDateTime] datetime2 NOT NULL,
        [UpdateDateTime] datetime2 NULL,
        CONSTRAINT [PK_Movies] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240229201249_InitialCreate'
)
BEGIN
    CREATE TABLE [Comments] (
        [Id] int NOT NULL IDENTITY,
        [Text] nvarchar(max) NOT NULL,
        [MovieId] int NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        [DateTime] datetime2 NOT NULL,
        CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Comments_Movies_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movies] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240229201249_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Comments_MovieId] ON [Comments] ([MovieId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240229201249_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240229201249_InitialCreate', N'8.0.2');
END;
GO


COMMIT;
GO
