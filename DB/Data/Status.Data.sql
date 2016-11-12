INSERT INTO dbo.Status (StatusId, Title, Comment, EntityType, StatusType)
SELECT 1, 'Active', 'Task Created but not started', 'Event', 'LifeCycle' UNION
SELECT 2, 'In Progress', 'Task is progressing to completion', 'Event', 'LifeCycle' UNION
SELECT 3, 'Paused', 'Task has been started but has stopped progressing', 'Event', 'LifeCycle' UNION
SELECT 4, 'Complete', 'Task is complete', 'Event', 'LifeCycle' UNION
SELECT 5, 'Deleted', 'Task is no longer relevant', 'Event', 'LifeCycle' UNION

SELECT 11, 'Active', 'Comment is visible', 'Comment', 'LifeCycle' UNION
SELECT 12, 'Deleted', 'Comment is deleted', 'Comment', 'LifeCycle'