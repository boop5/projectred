select 
	Id, Created, GameId, Price, Country
from 
	EzNintendo5.Nintendo.Trend
where
	Country = 'AU'
order by 
	CREATED DESC



delete from eznintendo5.nintendo.trend where country = 'AU' and Created > CAST('2020-02-10 19:00:00.0000000' as datetime2)
select * from eznintendo5.nintendo.trend where country = 'AU' and Created > CAST('2020-02-10 19:00:00.0000000' as datetime2)


select distinct AgeRatingType from EzNintendo5.Nintendo.Game 