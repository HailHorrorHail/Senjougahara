USE AzureSQL;
GO

IF OBJECT_ID (N'dbo.prc_AddEvent') IS NOT NULL
	DROP PROCEDURE dbo.prc_AddEvent
GO

CREATE PROCEDURE dbo.prc_AddEvent
	@ParentId		int = null,
	@Title			varchar(100),
	@Description	varchar(1000) = null,
	@Status			tinyint = null
AS
	DECLARE @ActiveEventStatus tinyint = 1;
	SET NOCOUNT ON;

	INSERT INTO dbo.Event (ParentId, Title, Description, StatusId, ModifiedDTim)
	VALUES (@ParentId, @Title, @Description, ISNULL(@Status, @ActiveEventStatus), GetUtcDate())

GO