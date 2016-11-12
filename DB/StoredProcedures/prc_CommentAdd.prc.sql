USE AzureSQL;
GO

IF OBJECT_ID (N'dbo.prc_AddComment') IS NOT NULL
	DROP PROCEDURE dbo.prc_AddComment
GO

CREATE PROCEDURE dbo.prc_AddComment
	@EventID		int = null,
	@Comment		varchar(100),
	@Status			tinyint = null
AS
	DECLARE @ActiveCommentStatus tinyint = 11;

	SET NOCOUNT ON;

	INSERT INTO dbo.Comment (EventId, Comment, StatusId, ModifiedDTim)
	VALUES (@EventId, @Comment, ISNULL(@Status, @ActiveCommentStatus), GetUtcDate())

GO