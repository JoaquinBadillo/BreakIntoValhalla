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
('deus', 'god@god.com', 'godmode');

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
SET AUTOCOMMIT=@OLD_AUTOCOMMIT;