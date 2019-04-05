USE master;
GO
IF DB_ID (N'StreamingDB') IS NOT NULL
DROP DATABASE StreamingDB;
GO

CREATE DATABASE StreamingDB;
GO

-- Verify the database files and sizes
SELECT name, size, size*1.0/128 AS [Size in MBs]
FROM sys.master_files
WHERE name = N'StreamingDB';
GO

USE StreamingDB;
GO


CREATE TABLE dbo.UserRewards (
    UserID VARCHAR(10) NOT NULL,
    PointBalance BIGINT NOT NULL,
    PointsRedeemed BIGINT NOT NULL,
    RowVer ROWVERSION,
    RowVerBigInt AS CAST(RowVer AS BIGINT)
)

CREATE TABLE dbo.UserRewardsTransactions (
    UserRewardTransactionID BIGINT IDENTITY(1,1),
    UserID VARCHAR(10) NOT NULL,
    Points BIGINT NOT NULL,
    Description VARCHAR(100) NOT NULL,
    Posted DATETIMEOFFSET NOT NULL CONSTRAINT DFLT_UserRewardsTransactions_Posted DEFAULT SYSDATETIMEOFFSET()
)

CREATE TABLE dbo.UserRewards_RealTime (
    UserID VARCHAR(10) NOT NULL,
    PointBalance BIGINT NOT NULL,
    PointsRedeemed BIGINT NOT NULL,
    RowVer ROWVERSION,
    RowVerBigInt AS CAST(RowVer AS BIGINT)
)


-- Let's just get some data in here for our awesome users
INSERT INTO dbo.UserRewards(UserID, PointBalance, PointsRedeemed)
VALUES
    ('User_1', 10000, 0), 
    ('User_2', 20000, 1000),
    ('User_3', 30000, 500),
    ('User_4', 40000, 250),
    ('User_5', 50000, 10000),
    ('User_6', 60000, 2050),
    ('User_7', 70000, 3250),
    ('User_8', 80000, 4125),
    ('User_9', 90000, 100)
    

SET NOCOUNT ON
DECLARE 
	@WaitTime INT,
	@UserId INT,
	@Seconds INT,
	@Milliseconds INT,
	@Delay VARCHAR(12),
	@Points INT
	
WHILE 1=1
BEGIN
	SET @WaitTime = CAST(RAND() * 1500 AS INT)
	SET @UserId = FLOOR(RAND() * 9 + 1)
	SET @Seconds = @WaitTime / 1000
	SET @Milliseconds = @WaitTime % 1000
	SET @Delay = CONCAT('00:00:0', @Seconds, '.', @Milliseconds)
	SET @Points = CAST( (-RAND() * 100) + (RAND() * 100) AS INT)

	WAITFOR DELAY @Delay

	-- Nobody wants a 0 point transaction!
	IF @Points <> 0
	BEGIN
		INSERT INTO dbo.UserRewardsTransactions(UserID, Points, Description)
		VALUES(
			CONCAT('User_', @UserId),
			@Points,
			CASE 
				WHEN @Points BETWEEN -100 AND -50 THEN 'Purchased Tech Gear' 
				WHEN @Points BETWEEN -49 AND -20 THEN  'Purchased Conference Ticket'
				WHEN @Points BETWEEN -19 AND 0 THEN 'Purchased Drinks'
				WHEN @Points BETWEEN 1 AND 20 THEN 'Earned via Social'
				WHEN @Points BETWEEN 21 AND 50 THEN 'Earned via Hard Work'
				WHEN @Points > 50 THEN 'Earned by Subscribing to Coding Blocks Podcast'
			END
		)
	END
END
