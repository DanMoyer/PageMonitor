CREATE TABLE [dbo].[AdHocPageStatus]
(
	[Id]				INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Url]				NVARCHAR(200) NOT NULL,
	[ResponseTime]		FLOAT  NOT NULL,
	[ContentLength]		INT  NOT NULL,
	[Status]			NVARCHAR(200) NOT NULL,
	[ExceptionMessage]	NVARCHAR(200) NULL,
	[Created]			DATETIME NOT NULL,
	[UserId]		    NVARCHAR(128) NOT NULL
	--CONSTRAINT FK_AdHocPageStatus_AspNetUsers_ID FOREIGN KEY  (UserId) REFERENCES  [dbo].[AspNetUsers](Id)
)
