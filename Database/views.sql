-- Net Kills View 
CREATE VIEW top_kills AS
SELECT users.user_id, users.username, metrics.kills
FROM users
INNER JOIN metrics USING (metrics_id)
ORDER BY metrics.kills DESC
LIMIT 10;

-- Weekly Eliminations Leaderboard 
-- (You only need to play that week to appear on the leaderboard)
CREATE VIEW top_weekly_elims AS
SELECT users.user_id, users.username, metrics.kills
FROM users
INNER JOIN metrics USING (metrics_id)
WHERE metrics.last_update >= DATE_SUB(CURDATE(), INTERVAL WEEKDAY(CURDATE()) DAY)
ORDER BY metrics.kills DESC;

-- Place of Death View
CREATE VIEW death_place AS
SELECT deaths.room, COUNT(deaths.room) AS "total"
FROM users
INNER JOIN deaths USING (user_id)
GROUP BY deaths.room;

-- Cause of Death View (Which type of character killed the player)
CREATE VIEW death_cause AS
SELECT deaths.killer, COUNT(deaths.killer) AS "total"
FROM users
INNER JOIN deaths USING (user_id)
GROUP BY deaths.killer;

