USE StreamingDB
GO
DECLARE @user varchar(20) = 'User_8'


;WITH CTE_SumPoints AS (
	SELECT 
		UserID, 
		SUM(Points) Points
	FROM dbo.UserRewardsTransactions
	GROUP BY UserID
),
CTE_PointsRedeemed AS (
	SELECT
		UserID,
		SUM(Points)*-1 Points
	FROM dbo.UserRewardsTransactions
	WHERE Points < 0
	GROUP BY UserID
)
SELECT ur.UserID,
	ur.PointBalance  AS StaringPointBalance,
	TransactionalPoints = sp.Points,
	CalculatedPointBalance = ur.PointBalance + sp.Points,
	urrt.PointBalance AS RealTimePointBalance,

	ur.PointsRedeemed AS StartingPointsRedeemed,
	TransactionalPointsRedeemed = pr.Points,
	CalculatedPointsRedeemed = ur.PointsRedeemed + pr.Points ,
	urrt.PointsRedeemed AS RealTimePointsRedeemed,

	ur.RowVerBigInt StaticRowVerBigint,
	urrt.RowVerBigInt RealTimeRowVerBigInt
FROM 
	dbo.UserRewards ur 
	LEFT JOIN dbo.UserRewards_RealTime urrt
		ON urrt.UserID = ur.UserID
	LEFT JOIN CTE_SumPoints sp 
		ON sp.UserID = ur.UserID
	LEFT JOIN CTE_PointsRedeemed pr
		ON pr.UserID = ur.UserID
ORDER BY ur.UserID


SELECT * 
FROM dbo.UserRewards ur
	LEFT JOIN dbo.UserRewards_RealTime urrt
		ON urrt.UserID = ur.UserID
ORDER BY 
	ur.UserID


SELECT * 
FROM 
	dbo.UserRewardsTransactions 
WHERE 
	UserID = @user 
	AND Points < 0
ORDER BY 
	UserRewardTransactionID