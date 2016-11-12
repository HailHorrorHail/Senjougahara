USE AzureSQL;
GO

IF OBJECT_ID (N'dbo.prc_FetchEvent') IS NOT NULL
	DROP PROCEDURE dbo.prc_FetchEvent
GO

CREATE PROCEDURE dbo.prc_FetchEvent
	@EventId	int
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
	WHERE e.EVentId = @EventId
	  AND e.StatusId != @DeletedEventStatus

GO