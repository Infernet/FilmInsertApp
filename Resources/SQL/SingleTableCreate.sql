create table Language
(
	LanguageId int identity primary key,
	LanguageName nvarchar(30) not null unique
);
go
create table Rated
(
	RatedId int identity primary key,
	RatedName nvarchar (30) not null unique
);
go
create table Country
(
CountryId int identity primary key,
CountryName nvarchar(30) not null unique
);
go
create table Production
(
ProductionId int identity primary key,
ProductionName nvarchar(30) not null unique
);
go
create table Genre
(
GenreId int identity primary key,
GenreName nvarchar(30) not null unique
);
go
create table RoleInMovie
(
RoleInMovieId int identity primary key,
RoleName nvarchar(30) not null unique
);
go
create table Person
(
PersonId int identity primary key,
FName nvarchar(20) not null,
LName nvarchar(30) not null,
Patronymic nvarchar(30) null
);
go
create table Movie
(
	MovieId int identity primary key,
	Title nvarchar(50) not null,
	YearReleaseWorld int not null,
	Rated int not null foreign key references Rated(RatedId),
	Released date not null,
	Runtime nvarchar(20) not null,
	Plot nvarchar(500) not null,
	RatingValue float not null,
	Metascore int null,
	ImdbRating float not null,
	ImdbVotes int not null,
	BoxOffice money null,
	DVD date not null,
	WebSite nvarchar(100) null
);
go
create table PersonInMovie
(
	PersonId int not null  foreign key references Person(PersonId),
	MovieId int not null  foreign key references Movie(MovieId),
	RoleInMovieId int not null  foreign key references RoleInMovie(RoleInMovieId),
	constraint PK_PersonToMovie primary key clustered  (PersonId,MovieId,RoleInMovieId)
);
go
create table LanguageInMovie
(
	MovieId int not null  foreign key references Movie(MovieId),
	LanguageId int not null  foreign key references Language(LanguageId),
	constraint PK_LanguageInMovie primary key clustered (MovieId,LanguageId)
);
go
create table MovieOnGenre
(
	MovieId int not null  foreign key references Movie(MovieId),
	GenreId int  not null foreign key references Genre(GenreId),
	constraint PK_MovieOnGenre primary key clustered (MovieId,GenreId)
);
go
create table MovieOnCountry
(
	CountryId int not null  foreign key references Country(CountryId),
	MovieId int  not null foreign key references Movie(MovieId),
	constraint PK_MovieOnCountry primary key clustered (CountryId,MovieId)
);
go
create table MovieOnProduction
(
MovieId int not null  foreign key references Movie(MovieId),
ProductionId int not null foreign key references Production(ProductionId),
constraint PK_MovieOnProduction primary key clustered (MovieId,ProductionId)
);
go
create table UserRole
(
RoleId int identity primary key,
Role nvarchar(20) not null unique
);
go
create table RegisteredUsers
(
Login nvarchar(20) not null unique,
Password nvarchar(20) not null,
RoleId int foreign key references UserRole(RoleId) not null,
constraint PK_RegisteredUsers primary key clustered (Login,Password) 
);