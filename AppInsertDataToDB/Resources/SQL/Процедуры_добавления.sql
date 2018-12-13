--Процедуры простых добавлений в таблицы с возвратом первичного ключа
create procedure InsertGenre
@GenreName nvarchar(30)
as
if exists(select * from Genre where Genre.GenreName=@GenreName)
	begin
	declare @result int
	select @result=Genre.GenreId from Genre where Genre.GenreName=@GenreName;
	return @result;
	end
else
	begin
	insert into Genre values (@GenreName);
	return @@identity;
	end;

go;

create procedure InsertLanguage
@LanguageName nvarchar(30)
as
if exists(select * from Language where Language.LanguageName=@LanguageName)
	Begin
	return select Language.LanguageId from Language where Language.LanguageName=@LanguageName;
	end
else
	begin
	insert into Language values (@LanguageName);
	return @@identity;
	end;

go;

create procedure InsertProduction
@ProductionName nvarchar(30)
as
if exists(select * from Production where Production.ProductionName=@ProductionName)
begin
declare @result int
select @result=Production.ProductionId from Production where Production.ProductionName=@ProductionName;
return @result;
end
else
begin
insert into Production values (@ProductionName);
return @@identity;
end;

go;

create procedure InsertCountry
@CountryName nvarchar(30)
as
if exists(select * from Country where Country.CountryName=@CountryName)
begin
declare @result int
select @result=Country.CountryId from Country where Country.CountryName=@CountryName;
return @result;
end
else
begin
insert into Country values (@CountryName);
return @@identity;
end;

go;

create procedure InsertPerson
@FName nvarchar(30),@LName nvarchar(30)
as
if exists(select * from Person where Person.FNameAndPatr=@FName and Person.LName=@LName)
begin
declare @result int
select @result=Person.PersonId from Person where Person.FNameAndPatr=@FName and Person.LName=@LName;
return @result;
end
else
begin
insert into Person(FNameAndPatr,LName) values (@FName,@LName);
return @@identity;
end;

go;

create procedure InsertRole
@RoleName nvarchar(30)
as
if exists(select * from RoleInMovie where RoleInMovie.RoleName=@RoleName)
	begin
		declare @result int
		select @result=RoleInMovieId from RoleInMovie where RoleName=@RoleName;
		return @result;
	end
else
begin
insert into RoleInMovie values (@RoleName);
return @@identity;
end;

go;