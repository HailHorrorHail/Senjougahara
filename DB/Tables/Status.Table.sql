IF OBJECT_ID (N'dbo.Status', N'U') IS NOT NULL 
BEGIN
	DROP TABLE dbo.Status
END;

GO

CREATE TABLE dbo.Status
(
	[StatusId]		int not null PRIMARY KEY,
	[Title]			varchar(50) not null,
	[Comment]		varchar(100),
	[EntityType]	varchar(100),
	[StatusType]	varchar(100)
)