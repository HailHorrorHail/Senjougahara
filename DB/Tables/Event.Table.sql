IF OBJECT_ID (N'dbo.Event', N'U') IS NOT NULL 
BEGIN
	DROP TABLE dbo.Event
END;

GO

CREATE TABLE dbo.Event
(
	[EventId]		int not null PRIMARY KEY IDENTITY(1, 1),
	[ParentId]		int,
	[Title]			varchar(100) not null,
	[Description]	varchar(1000),
	[Status]		tinyint not null,
	[CreatedDTim]	datetime DEFAULT(getutcdate()),
	[ModifiedDTim]	datetime not null,

	CONSTRAINT FK_Event_EventId FOREIGN KEY (ParentId)     
		REFERENCES dbo.Event (EventId)     
		ON DELETE NO ACTION    
		ON UPDATE NO ACTION
)