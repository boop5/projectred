USE [master]
CREATE LOGIN [RedSpider] WITH PASSWORD=N'enter secure password', 
						 DEFAULT_DATABASE=[eshop], 
						 CHECK_EXPIRATION=OFF, 
						 CHECK_POLICY=OFF
USE [eshop]
CREATE USER [RedSpider] FOR LOGIN [RedSpider]
USE [eshop]
ALTER USER [RedSpider] WITH DEFAULT_SCHEMA=[dbo]
USE [eshop]
ALTER ROLE [db_owner] ADD MEMBER [RedSpider]