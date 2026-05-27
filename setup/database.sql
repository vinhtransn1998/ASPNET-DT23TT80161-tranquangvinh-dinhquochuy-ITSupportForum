CREATE DATABASE ITSupportForumDb;
GO

USE ITSupportForumDb;
GO

CREATE TABLE Post
(
    Id INT PRIMARY KEY IDENTITY(1,1),

    Title NVARCHAR(MAX),

    Content NVARCHAR(MAX),

    ImageUrl NVARCHAR(MAX),

    CreatedAt DATETIME,

    UserId NVARCHAR(MAX)
);
GO

CREATE TABLE Comment
(
    Id INT PRIMARY KEY IDENTITY(1,1),

    Content NVARCHAR(MAX),

    PostId INT,

    CreatedAt DATETIME,

    UserId NVARCHAR(MAX)
);
GO