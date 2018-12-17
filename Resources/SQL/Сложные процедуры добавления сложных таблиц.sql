--Процедуры для связования таблиц связей многие-ко-многим

create procedure InsertMovieOnGenre
@FilmId int,@GenreValue nvarchar(30)
as
begin
	declare @genreId int;
	execute @genreId=InsertGenre @GenreValue;
	if not exists(select * from MovieOnGenre where MovieOnGenre.GenreId=@genreId and MovieOnGenre.MovieId=@FilmId)
		insert into MovieOnGenre values(@FilmId,@genreId);
end;

go;

create procedure InsertMovieOnProduction
@FilmId int,@ProductionValue nvarchar(30)
as
begin
	declare @productionId int;
	execute @productionId= InsertProduction @ProductionValue;
	if not exists(select * from MovieOnProduction where MovieOnProduction.MovieId=@FilmId and MovieOnProduction.ProductionId=@productionId)
		insert into MovieOnProduction (MovieId,ProductionId) values(@FilmId,@productionId);
end;

go;

create procedure InsertMovieOnLanguage
@FilmId int, @LanguageValue nvarchar(30)
as
begin
	declare @languageId int;
	execute @languageId=InsertLanguage @LanguageValue;
	if not exists(select * from LanguageInMovie where LanguageInMovie.MovieId=@FilmId and LanguageInMovie.LanguageId=@languageId)
		insert into LanguageInMovie(MovieId,LanguageId) values(@FilmId,@languageId);
end;

go;

create procedure InsertMovieOnCountry
@FilmId int,@CountryValue nvarchar(30)
as
begin
	declare @countryId int;
	execute @countryId = InsertCountry @CountryValue;
	if not exists(select * from MovieOnCountry where MovieOnCountry.MovieId=@FilmId and MovieOnCountry.CountryId=@countryId)
		insert into MovieOnCountry (MovieId,CountryId) values(@FilmId,@countryId);
end;

go;

create procedure InsertPersonInMovie
@FilmId int,@FName nvarchar(30),@LName nvarchar(30),@RoleName nvarchar(30)
as
begin
	declare @RoleId int,@PersonId int;
	execute @RoleId = InsertRole @RoleName;
	execute @PersonId = InsertPerson @FName,@LName;
	if not exists (select * from PersonInMovie where PersonId=@PersonId and MovieId=@FilmId and RoleInMovieId=@RoleId)
		insert into PersonInMovie (MovieId,PersonId,RoleInMovieId) values (@FilmId,@PersonId,@RoleId);
end;

go;

