-- Valhalla Database Triggers
-- Version 1.0
-- Triggers that allow the deletion of users.

-- Last edited June 05, 2023
-- Joaquín Badillo

DELIMITER $$
    CREATE TRIGGER `valhalla`.`delete_level`
    AFTER DELETE ON `valhalla`.`games`
    FOR EACH ROW
    BEGIN
        DELETE FROM valhalla.levels WHERE levels.level_id = OLD.level_id;
    END$$
DELIMITER ;

DELIMITER $$
    CREATE TRIGGER `valhalla`.`delete_metrics`
    AFTER DELETE ON `valhalla`.`users`
    FOR EACH ROW
    BEGIN
        DELETE FROM valhalla.metrics WHERE metrics.metrics_id = OLD.metrics_id;
    END$$
DELIMITER ;

DELIMITER $$
    CREATE TRIGGER `valhalla`.`delete_user_game`
    AFTER DELETE ON `valhalla`.`users`
    FOR EACH ROW
    BEGIN
        DELETE FROM valhalla.games WHERE games.game_id = OLD.game_id;
    END$$
DELIMITER ;

DELIMITER $$
    CREATE TRIGGER `valhalla`.`delete_user_deaths`
    AFTER DELETE ON `valhalla`.`users`
    FOR EACH ROW
    BEGIN
        DELETE FROM valhalla.deaths WHERE deaths.user_id = OLD.user_id;
    END$$
DELIMITER ;