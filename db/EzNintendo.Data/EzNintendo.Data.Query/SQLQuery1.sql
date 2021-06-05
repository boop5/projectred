SELECT [t].[Id], [t].[Country], [t].[Created], [t].[Discount], [t].[GameId], [t].[Price], [t0].[Id], [t0].[Country], [t0].[Created], [t0].[Discount], [t0].[GameId], [t0].[Price]
FROM [Nintendo].[Trend] AS [t]
INNER JOIN [Nintendo].[Trend] AS [t0] ON ([t].[GameId] = [t0].[GameId]) AND ((([t].[Country] = [t0].[Country]) AND ([t].[Country] IS NOT NULL AND [t0].[Country] IS NOT NULL)) OR ([t].[Country] IS NULL AND [t0].[Country] IS NULL))
WHERE ([t].[Created] > [t0].[Created]) 