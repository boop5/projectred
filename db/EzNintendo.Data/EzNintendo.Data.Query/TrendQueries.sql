--SELECT 
--	g.Title, t.Price
--FROM
--	Nintendo.Game AS g
--INNER JOIN
--	Nintendo.Trend AS t
--INNER JOI
--ON 
--	g.Id = t.GameId
--WHERE
--	t.Created < DATEADD(MINUTE, -65, CURRENT_TIMESTAMP)

-- =============================================================================================

SELECT
	g.Title, 
	CAST(p.Price AS money) AS RegularPrice,
	CAST(t.Price AS money) AS CurrentPrice,	
	--CASE
	--	WHEN t.Price = 0 THEN 'N/A'
	--	ELSE CAST(ROUND(100 - (t.Price / (p.Price / 100)), 0) AS nvarchar)
 --   END AS DiscountX,	
	t.Discount, 
	t.Country
FROM
	Nintendo.Trend AS t
LEFT JOIN
	Nintendo.Game AS g
ON
	g.Id = t.GameId
LEFT JOIN
	Nintendo.RegularPrice AS p
ON
	p.GameId = t.GameId AND p.Country = t.Country
--WHERE
--	g.Title = 'Pokémon Mystery Dungeon: Rescue Team DX'
	--t.Price != p.Price
ORDER BY
	 g.title, t.Country


-- =============================================================================================


--SELECT 
--	COUNT(*) 
--FROM 
--	Nintendo.Trend


-- =============================================================================================


--DELETE FROM Nintendo.Trend


SELECT Country, COUNT(Country) AS C FROM Nintendo.Trend GROUP BY Country ORDER BY Country, C DESC


