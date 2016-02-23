SELECT ResponseTime, Count(*) TotalCount FROM 
( SELECT ResponseTime = CASE  
							WHEN ResponseTime >= 0 and ResponseTime <= 1 THEN '0-1' 
							WHEN ResponseTime >= 1 and ResponseTime <= 5 THEN '1-5' 
							WHEN ResponseTime >= 5 and ResponseTime <= 10 THEN '6-10' 
							WHEN ResponseTime >= 10 and ResponseTime <= 15 THEN '10-15' 
							WHEN ResponseTime >= 15 and ResponseTime <= 20 THEN '15-20'
							WHEN ResponseTime >= 20 and ResponseTime <= 25 THEN '20-25' 
							ELSE 'over 25' 
							END 
	FROM[PageMonitor].[dbo].[PageStatus] 
	WHERE Url = 'https://www.findlay.edu' 
	AND Status = 'OK' 
	AND Created >= '2/8/2016'  AND Created <= '2/22/2016' 
	GROUP BY ResponseTime) AS SourceTabel 
	GROUP BY ResponseTime 


	  --calculate count of records 

	SELECT  Count(*) TotalCount	FROM[PageMonitor].[dbo].[PageStatus] 
	WHERE Url = 'https://www.findlay.edu' 
	AND Status = 'OK' 
	AND Created >= '2/8/2016'  AND Created <= '2/22/2016' 