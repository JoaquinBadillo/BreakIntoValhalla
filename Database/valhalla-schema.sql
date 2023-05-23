-- Valhalla Database Schema
-- Version 1.0
-- Developed by Einherjar / Joaquín Badillo, Pablo Bolio, Shaul Zayat


-- The database can properly handle and store any type of character
SET NAMES utf8mb4; 

-- Temporarily disables the verification of single key restrictions
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;

-- Temporarily disables foreign key constraint checking
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;

-- MySQL will apply stricter validation rules and generate errors for operations that do not comply with these rules
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';


DROP SCHEMA IF EXISTS valhalla;
CREATE SCHEMA valhalla;
USE valhalla;

--
-- Table structure for table `users`
--
CREATE TABLE users (
	user_id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    username VARCHAR(30) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(32) NOT NULL,
    PRIMARY KEY (user_id)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;


--
-- Table structure for table `metrics`
--
CREATE TABLE metrics (
	metrics_id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    user_id SMALLINT UNSIGNED NOT NULL,
    kills SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    deaths SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    wins SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    last_update TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (metrics_id),
    CONSTRAINT fk_users_metrics FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


--
-- Table structure for table `levels`
--
CREATE TABLE levels (
	level_id TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
    -- In case the size of the number generated from the seed is large
    seed MEDIUMINT UNSIGNED NOT NULL,
    PRIMARY KEY (level_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


--
-- Table structure for table `classes`
--
CREATE TABLE classes (
	class_id TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
    name VARCHAR(20) NOT NULL,
    PRIMARY KEY (class_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


--
-- Table structure for table `games`
--
CREATE TABLE games (
	game_id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    user_id SMALLINT UNSIGNED NOT NULL,
    level_id TINYINT UNSIGNED NOT NULL,
    class_id TINYINT UNSIGNED NOT NULL,
    enemy_stats_multiplier FLOAT NOT NULL DEFAULT 1,
    PRIMARY KEY (game_id),
    CONSTRAINT fk_users_game FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_levels_game FOREIGN KEY (level_id) REFERENCES levels (level_id) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_classes_game FOREIGN KEY (class_id) REFERENCES classes (class_id) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;


--
-- Table structure for table `sats`
--
CREATE TABLE stats (
	stats_id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    class_id TINYINT UNSIGNED NOT NULL,
    hp SMALLINT UNSIGNED NOT NULL,
    attack SMALLINT UNSIGNED NOT NULL,
    attack_speed FLOAT NOT NULL,
    defense SMALLINT UNSIGNED NOT NULL,
    speed FLOAT NOT NULL,
    PRIMARY KEY (stats_id),
    CONSTRAINT fk_classes_stats FOREIGN KEY (class_id) REFERENCES classes (class_id) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

--
-- Table structure for table `deaths`
--
CREATE TABLE deaths(
	deaths_id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    user_id SMALLINT UNSIGNED NOT NULL,
    room VARCHAR(50) NOT NULL,
	killer VARCHAR(50),
    PRIMARY KEY (deaths_id),
	CONSTRAINT fk_deaths_users FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE RESTRICT ON UPDATE CASCADE
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;



SHOW TABLES;

-- Enable restrictions
SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;