SELECT
	*
FROM
	Nintendo.Game AS g
WHERE
	g.Title = 'Pok�mon Mystery Dungeon: Rescue Team DX'

--DELETE 
--FROM 
--	Nintendo.Game
--WHERE 
--	Title = 'Pok�mon Mystery Dungeon: Rescue Team DX'


SELECT COUNT(*) AS CountGames FROM Nintendo.Game


select s.name as schema_name, 
    s.schema_id,
    u.name as schema_owner
from sys.schemas s
    inner join sys.sysusers u
        on u.uid = s.principal_id
order by s.name