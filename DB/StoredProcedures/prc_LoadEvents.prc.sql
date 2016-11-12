USE AzureSQL;
GO

IF OBJECT_ID (N'dbo.prc_LoadEvents') IS NOT NULL
	DROP PROCEDURE dbo.prc_LoadEvents
GO

CREATE PROCEDURE dbo.prc_LoadEvents
AS
	DECLARE @DeletedEventStatus tinyint = 5;

	SELECT	[EventId],
			[ParentId],
			[Title],
			[Description],
			[StatusId],
			[CreatedDTim],
			[ModifiedDTim]
	FROM dbo.Event e with (nolock)
	WHERE c.EventId = @EventId
	  AND c.StatusId != @DeletedCommentStatus

GO