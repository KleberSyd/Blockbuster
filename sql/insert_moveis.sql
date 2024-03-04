
-- Ensure the script uses the correct database
USE blockbuster_movies;
GO

-- Declare a variable to hold the current date
DECLARE @tmp DATETIME = GETDATE();

-- Define a generic procedure for inserting movies if they don't already exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'InsertMovieIfNotExists')
BEGIN
    EXEC('CREATE PROCEDURE InsertMovieIfNotExists
        @Title nvarchar(max),
        @Genre nvarchar(max),
        @Director nvarchar(max),
        @Year int,
        @Rating int,
        @ImageUrl nvarchar(max),
        @TrailerUrl nvarchar(max),
        @Description nvarchar(max),
        @IsAvailable bit,
        @Timespan int,
        @CreateDateTime datetime2,
        @UpdateDateTime datetime2
    AS
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM Movies WHERE Title = @Title)
        BEGIN
            INSERT INTO Movies (Title, Genre, Director, Year, Rating, ImageUrl, TrailerUrl, Description, IsAvailable, Timespan, CreateDateTime, UpdateDateTime)
            VALUES (@Title, @Genre, @Director, @Year, @Rating, @ImageUrl, @TrailerUrl, @Description, @IsAvailable, @Timespan, @CreateDateTime, @UpdateDateTime);
        END
    END');
END;



-- Insert movies with creative details
EXEC InsertMovieIfNotExists N'Parisian Love Rhapsody', 'Romance/Comedy', 'Luc Besson', 2023, 8, '/movie-images/a_romantic_comedy_movie_poster_set_in_paris.webp', 'https://example.com/trailer1', 'In the heart of Paris, two lovers find each other through comedic misunderstandings, with the Eiffel Tower witnessing their blossoming romance.', 1, 120, @tmp, NULL;
EXEC InsertMovieIfNotExists N'Echoes of the Asylum', 'Horror', 'Guillermo Del Toro', 2022, 7, '/movie-images/asylum_shadows.webp', 'https://example.com/trailer2', 'An investigative journalist uncovers chilling secrets within the walls of an abandoned asylum, where the past haunts the present.', 1, 105, @tmp, NULL;
EXEC InsertMovieIfNotExists N'The Dragon Crest', 'Fantasy/Adventure', 'Peter Jackson', 2025, 9, '/movie-images/dragons_peak.webp', 'https://example.com/trailer3', 'A band of heroes embarks on a quest to find the Dragon Crest, battling through mystical lands shadowed by a legendary beast.', 1, 130, @tmp, NULL;
EXEC InsertMovieIfNotExists N'Stars of Eternity', 'Sci-Fi', 'Ridley Scott', 2024, 5, '/movie-images/galactic_requiem.webp', 'https://example.com/trailer4', 'In the vastness of space, a rebel fleet battles against a cosmic empire, with the fate of the universe hanging in the balance.', 1, 140, @tmp, NULL;
EXEC InsertMovieIfNotExists N'Invasion: Earth', 'Sci-Fi/Action', 'Michael Bay', 2023, 7, '/movie-images/invasion_battle.webp', 'https://example.com/trailer5', 'When Earth faces an alien threat, it''s up to a ragtag team of soldiers and scientists to defend our home against the impossible.', 1, 115, @tmp, NULL;
EXEC InsertMovieIfNotExists N'Heist Under the Neon Lights', 'Action', 'Kathryn Bigelow', 2021, 6, '/movie-images/love_a_la_parisienne.webp', 'https://example.com/trailer6', 'A master thief and her crew plan the ultimate heist in a neon-lit metropolis, but betrayal lurks at every corner.', 1, 125, @tmp, NULL;
EXEC InsertMovieIfNotExists N'Mist of the Noir', 'Noir', 'David Fincher', 2025, 10, '/movie-images/shadows_in_the_fog.webp', 'https://example.com/trailer7', 'In a city veiled by fog, a detective unravels a mystery entangled in the shadows of crime and corruption.', 1, 180, @tmp, NULL;
EXEC InsertMovieIfNotExists N'The Enchanted Forest', 'Animated', 'Hayao Miyazaki', 2022, 9, '/movie-images/whispering_woods.webp', 'https://example.com/trailer8', 'A young girl discovers a magical forest where talking animals teach her about friendship', 1, 90, @tmp, NULL;
GO