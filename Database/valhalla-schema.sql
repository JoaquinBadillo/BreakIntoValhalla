-- Valhalla Database Schema
-- Version 2.0
-- Last edited June 05, 2023

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
-- Table structure for table `metrics`
--
CREATE TABLE metrics (
	metrics_id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    kills SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    wins SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    last_update TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (metrics_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Table structure for table `levels`
--
CREATE TABLE levels (
	level_id TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
    level_num TINYINT UNSIGNED NOT NULL DEFAULT 1,
    seed MEDIUMINT UNSIGNED NOT NULL DEFAULT 0,
    PRIMARY KEY (level_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Table structure for table `characters`
--
CREATE TABLE characters (
    character_id TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
    class_id TINYINT UNSIGNED NOT NULL,
    gender VARCHAR(20) NOT NULL,
    PRIMARY KEY (character_id),
    INDEX class_id (class_id ASC),
    CONSTRAINT fk_characters_classes FOREIGN KEY (class_id) REFERENCES classes (class_id) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Table structure for table `classes`
--
CREATE TABLE classes (
	class_id TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
    name VARCHAR(20) NOT NULL,
    stats_id SMALLINT UNSIGNED NOT NULL,
    PRIMARY KEY (class_id),
    UNIQUE INDEX name (name ASC),
    UNIQUE INDEX stats_id (stats_id ASC),
    CONSTRAINT fk_classes_stats FOREIGN KEY (stats_id) REFERENCES stats (stats_id) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Table structure for table `stats`
--
CREATE TABLE stats (
	stats_id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    hp SMALLINT UNSIGNED NOT NULL,
    primary_attack SMALLINT UNSIGNED NOT NULL,
    secondary_attack SMALLINT NOT NULL,
    primary_lag FLOAT NOT NULL,
    secondary_lag FLOAT NOT NULL,
    defense SMALLINT UNSIGNED NOT NULL,
    speed FLOAT NOT NULL,
    PRIMARY KEY (stats_id)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

--
-- Table structure for table `games`
--
CREATE TABLE games (
	game_id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    level_id TINYINT UNSIGNED DEFAULT NULL,
    character_id TINYINT UNSIGNED DEFAULT NULL,
    PRIMARY KEY (game_id),
    UNIQUE INDEX level_id (level_id ASC),
    INDEX character_id (character_id ASC),
    CONSTRAINT fk_levels_game FOREIGN KEY (level_id) REFERENCES levels (level_id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_character_id FOREIGN KEY (character_id) REFERENCES characters (character_id) ON DELETE RESTRICT
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

--
-- Table structure for table `users`
--
CREATE TABLE users (
	user_id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    username VARCHAR(30) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(32) NOT NULL,
    metrics_id SMALLINT UNSIGNED NOT NULL,
    game_id SMALLINT UNSIGNED DEFAULT NULL,
    PRIMARY KEY (user_id),
    UNIQUE INDEX metrics_id (metrics_id ASC),
    UNIQUE INDEX game_id (game_id ASC),
    UNIQUE INDEX username (username ASC),
    CONSTRAINT fk_users_metrics FOREIGN KEY (metrics_id) REFERENCES metrics (metrics_id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_users_game FOREIGN KEY (game_id) REFERENCES games (game_id) ON DELETE CASCADE ON UPDATE CASCADE
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
	FOREIGN KEY (user_id) REFERENCES users (user_id) ON UPDATE CASCADE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

-- Enable restrictions
SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;