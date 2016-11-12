USE AzureSQL;
GO

IF OBJECT_ID (N'dbo.prc_LoadComments') IS NOT NULL
	DROP PROCEDURE dbo.prc_LoadComments
GO

CREATE PROCEDURE dbo.prc_LoadComments
	@EventId	int
AS
	DECLARE @DeletedCommentStatus tinyint = 12;

	SELECT	[CommentId],
			[EventId],
			[Comment],
			[StatusId],
			[CreatedDTim],
			[ModifiedDTim]
	FROM dbo.Comment c with (nolock)
	WHERE c.EventId = @EventId
	  AND c.StatusId != @DeletedCommentStatus

GO