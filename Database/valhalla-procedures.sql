-- Valhalla Database Procedures
-- Version 1.0
-- Procedures that facilitate the creation of users and games

-- Last edited June 05, 2023
-- Joaquín Badillo

USE valhalla;

DELIMITER $$
    CREATE PROCEDURE valhalla.create_user(
        IN username VARCHAR(30),
        IN email VARCHAR(50),
        IN password VARCHAR(50))
    BEGIN
        IF (SELECT COUNT(*) FROM valhalla.users WHERE users.username = username OR users.email = email) > 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'User already exists';
        END IF;
        
        INSERT INTO valhalla.metrics (`wins`) VALUES (0);
        INSERT INTO valhalla.users (`username`, `email`, `password`, `metrics_id`) VALUES
        (username, email, password, LAST_INSERT_ID()); 
    END$$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE valhalla.create_game(
        IN username VARCHAR(30),
        IN character_id TINYINT UNSIGNED,
        IN seed MEDIUMINT UNSIGNED)
    BEGIN
		IF (SELECT COUNT(*) FROM valhalla.users WHERE users.username = username) = 0 THEN
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid User!';
		END IF;
		
        IF (SELECT COUNT(*) FROM valhalla.users JOIN valhalla.games USING (game_id) WHERE users.username = username) > 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Game Already Exists!';
        END IF;
        
        INSERT INTO valhalla.levels (`level_num`, `seed`) VALUES (1, seed);
        SET @last_level_id = LAST_INSERT_ID();
        INSERT INTO valhalla.games (`level_id`, `character_id`) VALUES (@last_level_id, character_id);
        SET @last_game_id = LAST_INSERT_ID();
        UPDATE valhalla.users SET users.game_id = @last_game_id WHERE users.username = username;
    END$$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE valhalla.add_death(
        IN _username VARCHAR(30),
        IN _room VARCHAR(50),
        IN _killer VARCHAR(50))
    BEGIN
        IF (SELECT COUNT(*) FROM valhalla.users WHERE users.username = _username) = 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid User!';
        END IF;

        INSERT INTO deaths (`user_id`, `room`, `killer`) VALUES 
        ((SELECT users.user_id FROM valhalla.users WHERE users.username = _username), _room, _killer);
    END$$
DELIMITER ;
