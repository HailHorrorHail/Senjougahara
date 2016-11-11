IF OBJECT_ID (N'dbo.Comment', N'U') IS NOT NULL 
BEGIN
	DROP TABLE dbo.Comment
END;

GO

CREATE TABLE dbo.Comment
(
	[CommentId]		int not null PRIMARY KEY IDENTITY(1, 1),
	[EventId]		int not null,
	[Comment]		varchar(1000) not null,
	[Status]		tinyint not null,
	[CreatedDTim]	datetime DEFAULT(getutcdate()),

	CONSTRAINT FK_Comment_EventId FOREIGN KEY (EventId)     
		REFERENCES dbo.Event (EventId)     
		ON DELETE CASCADE  
		ON UPDATE NO ACTION
)