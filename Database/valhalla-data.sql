-- Valhalla Sample Database Data
--- This data will be used for testing

SET NAMES utf8mb4;
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';
SET @old_autocommit=@@autocommit;

USE valhalla;

SET AUTOCOMMIT=0;
INSERT INTO users (`username`, `email`, `password`) VALUES 
('user1', 'user1@test.com', 'secret!'),
('user2', 'user2@test.com', 'secreterSecret'),
('user3', 'user3@test.com', 'superSecret'),
('user4', 'user4@test.com', 'shhhh!'),
('user5', 'user5@test.com', 'password'),
('user6', 'user6@test.com', '123456'),
('thor', 'odins@favourite.com', 'jotunHater1');
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO metrics (`user_id`, `kills`, `deaths`, `wins`) VALUES
(1, 100, 5, 2),
(2, 50, 10, 0),
(3, 200, 2, 5),
(4, 20, 20, 0),
(5, 150, 5, 3),
(6, 50, 15, 0),
(7, 1000, 0, 100);
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO levels (`seed`) VALUES
(123456),
(654321),
(987654),
(123789),
(456123),
(789456),
(159753);
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO classes (`name`) VALUES
('Archer'),
('Berserker'),
('Spellcaster');
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO games (`user_id`, `level_id`, `class_id`, `enemy_stats_multiplier`) VALUES
(1, 1, 1, 1),
(2, 2, 2, 1),
(3, 3, 3, 1),
(4, 4, 1, 1),
(5, 5, 2, 1),
(6, 6, 3, 1),
(7, 1, 1, 1);
COMMIT;
 
SET AUTOCOMMIT=0; 
INSERT INTO stats (`class_id`, `hp`, `attack`, `attack_speed`, `defense`, `speed`) VALUES
(1, 100, 10, 1.0, 5, 15.0),
(2, 200, 25, 1.5, 1, 20.0),
(3, 100, 15, 1.0, 2, 10.0);
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO deaths (`user_id`, `room`, `killer`) VALUES
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