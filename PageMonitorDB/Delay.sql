CREATE TABLE [dbo].[Delays]
(
	[Id]				SMALLINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[IterationHour]		SMALLINT NOT NULL,
	[IterationMinute]	SMALLINT NOT NULL,
	[IterationSecond]	SMALLINT NOT NULL,
	[PageHour]			SMALLINT NOT NULL,
	[PageMinute]		SMALLINT NOT NULL,
	[PageSecond]		SMALLINT NOT NULL


)
