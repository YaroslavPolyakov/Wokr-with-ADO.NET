CREATE TABLE Sitecore
(
Id int primary key,
ItemId UNIQUEIDENTIFIER DEFAULT NEWID(),
EntityId varchar(10) not null,
Type varchar(10) not null
)