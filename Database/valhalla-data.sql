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
INSERT INTO metrics (`user_id`, `kills`, `wins`) VALUES
(1, 100, 2),
(2, 50, 0),
(3, 200, 5),
(4, 20, 0),
(5, 150, 3),
(6, 50, 0),
(7, 1000, 100);
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO levels (`level_num`,`seed`) VALUES
(1, 123456),
(1, 654321),
(1, 987654),
(1, 123789),
(1, 456123),
(1, 789456),
(1, 159753);
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO classes (`name`) VALUES
('Archer'),
('Berserker'),
('Spellcaster');
COMMIT;

SET AUTOCOMMIT=0;
INSERT INTO games (`user_id`, `level_id`, `class_id`) VALUES
(1, 1, 1),
(2, 2, 2),
(3, 3, 3),
(4, 4, 1),
(5, 5, 2),
(6, 6, 3),
(7, 7, 1);
COMMIT;

 
SET AUTOCOMMIT=0; 
INSERT INTO stats (`class_id`, `hp`, `primary_attack`, `secondary_attack`, `primary_lag`, `secondary_lag`, `defense`, `speed`) VALUES
(1, 200, 20, 15, 0.5, 1.5, 5, 2),
(2, 250, 30, 40, 0.5, 1.1, 2, 3),
(3, 150, 15, 35, 0.5, 2, 4, 2);
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
