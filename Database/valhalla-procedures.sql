DELIMITER $$
    CREATE PROCEDURE init_user(
        IN username VARCHAR(30),
        IN email VARCHAR(50),
        IN password VARCHAR(50))
    BEGIN
        IF (SELECT COUNT(*) FROM valhalla.users WHERE users.username = username OR users.email = email) > 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'User already exists';
        END IF;
        INSERT INTO valhalla.levels (`level_num`) VALUES (1);
        INSERT INTO valhalla.games (`level_id`) VALUES (LAST_INSERT_ID());
        SET @last_game_id = LAST_INSERT_ID();
        INSERT INTO valhalla.metrics (`wins`) VALUES (0);
        INSERT INTO valhalla.users (`username`, `email`, `password`, `metrics_id`, `game_id`) VALUES
        (username, email, password, LAST_INSERT_ID(), @last_game_id);
    END$$
DELIMITER ;