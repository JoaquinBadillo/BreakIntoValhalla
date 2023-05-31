-- Valhalla Sample Database Data
--- This data will be used for testing

SET NAMES utf8mb4;
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';
SET @old_autocommit=@@autocommit;

USE valhalla;

SET AUTOCOMMIT=0;
INSERT INTO valhalla.users (`username`, `email`, `password`, `metrics_id`, `game_id`) VALUES 
('user1', 'user1@test.com', 'secret!', 1, 1),
('user2', 'user2@test.com', 'secreterSecret', 2, 2),
('user3', 'user3@test.com', 'superSecret', 3, 3),
('user4', 'user4@test.com', 'shhhh!', 4, 4),
('user5', 'user5@test.com', 'password',5 ,5),
('user6', 'user6@test.com', '123456', 6, 6),
('thor', 'odins@favourite.com', 'jotunHater1', 7, 7);
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO valhalla.metrics (`kills`, `wins`) VALUES
(100, 2),
(50, 0),
(200, 5),
(20, 0),
(150, 3),
(50, 0),
(1000, 100);
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO valhalla.levels (`level_num`,`seed`) VALUES
(1, 123456),
(1, 654321),
(1, 987654),
(1, 123789),
(1, 456123),
(1, 789456),
(1, 159753);
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO valhalla.characters (`class_id`, `gender`) VALUES
(1, "Female"),
(1, "Male"),
(2, "Female"),
(2, "Male"),
(3, "Neutral");
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO valhalla.classes (`name`, `stats_id`) VALUES
('Archer', 1),
('Berserker', 2),
('Spellcaster', 3);
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO valhalla.games (`level_id`, `character_id`) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 4);
COMMIT;

SET AUTOCOMMIT=0; 
INSERT INTO valhalla.stats (`hp`, `primary_attack`, `secondary_attack`, `primary_lag`, `secondary_lag`, `defense`, `speed`) VALUES
(200, 20, 15, 0.5, 1.5, 5, 2),
(250, 30, 40, 0.5, 1.1, 2, 3),
(150, 15, 35, 0.5, 2, 4, 2);
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO valhalla.deaths (`user_id`, `room`, `killer`) VALUES
(1, 'Big Battle', 'Sword Draugr'),
(2, 'Hard Battle', 'Archer Draugr'),
(3, 'Hard Battle', 'Archer Draugr'),
(4, 'Hel Boss Room', 'Hel'),
(5, 'Hel Boss Room', 'Hel'),
(6, 'Hel Boss Room', 'Sword Draugr');
COMMIT;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
SET AUTOCOMMIT=@OLD_AUTOCOMMIT;
