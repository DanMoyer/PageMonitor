CREATE TABLE [dbo].[AdHocPageStatus]
(
	[Id]				INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Url]				NVARCHAR(200) NOT NULL,
	[ResponseTime]		FLOAT  NOT NULL,
	[ContentLength]		INT  NOT NULL,
	[Status]			NVARCHAR(200) NOT NULL,
	[ExceptionMessage]	NVARCHAR(200) NULL,
	[Created]			DATETIME NOT NULL,
	[User]				NVARCHAR(128) NOT NULL
)
