-- Valhalla Database Initialization
-- Version 1.0
-- Contains only the character's data

-- Last edited June 05, 2023
-- Joaquín Badillo


SET NAMES utf8mb4;
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';
SET @old_autocommit=@@autocommit;

USE valhalla;

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
INSERT INTO valhalla.stats (`hp`, `primary_attack`, `secondary_attack`, `primary_lag`, `secondary_lag`, `defense`, `speed`) VALUES
(200, 20, 15, 0.5, 1.5, 5, 5),
(250, 30, 40, 0.5, 1.1, 2, 6),
(150, 15, 35, 0.5, 2, 4, 5);
COMMIT;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
SET AUTOCOMMIT=@OLD_AUTOCOMMIT;