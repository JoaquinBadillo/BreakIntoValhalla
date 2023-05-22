-- Net Kills View 
CREATE VIEW top_kills AS
SELECT users.user_id, users.username, metrics.kills
FROM users
INNER JOIN metrics ON users.user_id = metrics.user_id
ORDER BY metrics.kills DESC;

-- K/D View
CREATE VIEW top_kd AS
SELECT users.user_id, users.username, metrics.kills, metrics.deaths
FROM users
INNER JOIN metrics ON users.user_id = metrics.user_id
ORDER BY metrics.kills/metrics.deaths DESC;

-- Win Rate View
CREATE VIEW top_winrate AS
SELECT users.user_id, users.username, metrics.wins, metrics.deaths
FROM users
INNER JOIN metrics ON users.user_id = metrics.user_id
ORDER BY metrics.wins/metrics.deaths DESC;

-- Weekly Eliminations Leaderboard 
-- (You only need to play that week to appear on the leaderboard)
CREATE VIEW top_weekly_elims AS
SELECT users.user_id, users.username, metrics.kills
FROM users
INNER JOIN metrics ON users.user_id = metrics.user_id
WHERE metrics.last_update >= DATE_SUB(CURDATE(), INTERVAL WEEKDAY(CURDATE()) DAY)
ORDER BY metrics.kills DESC;