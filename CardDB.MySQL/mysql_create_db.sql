CREATE DATABASE IF NOT EXISTS `cards`
DEFAULT CHARACTER SET utf8 COLLATE utf8_bin;


use cards;


CREATE TABLE IF NOT EXISTS `Action`
(
	`ID` bigint(20) NOT NULL,
	`SystemID` char(12) COLLATE utf8_bin NOT NULL,
	`Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	`Modified` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	`Type` enum('CreateCard','ModifyCard','DeleteCard','CreateView','ModifyView','DeleteView') COLLATE utf8_bin NOT NULL,
	`Data` longtext COLLATE utf8_bin NOT NULL,
	
	
	PRIMARY KEY (`ID`),
	
	UNIQUE KEY `u_SystemID` (`SystemID`),
	KEY `k_Created` (`Created`),
	KEY `k_Modified` (`Modified`)
) 
ENGINE=InnoDB 
DEFAULT CHARSET=utf8 COLLATE=utf8_bin;


CREATE TABLE IF NOT EXISTS `Item`
(
	`ID` char(12) COLLATE utf8_bin NOT NULL,
	`SequenceID` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	`Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	`Modified` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	`Type` enum('View','Card') COLLATE utf8_bin NOT NULL,
	`Data` longtext COLLATE utf8_bin NOT NULL,
	
	
	PRIMARY KEY (`ID`),
	
	KEY `k_SequenceID` (`SequenceID`),
	KEY `k_Created` (`Created`),
	KEY `k_Modified` (`Modified`)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8 COLLATE=utf8_bin;


CREATE TABLE IF NOT EXISTS `DBVersions`
(
  `Version` int(11) NOT NULL,
  `Created` datetime NOT NULL,
  
  
  PRIMARY KEY (`Version`),
  
  KEY `k_Created` (`Created`)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8 COLLATE=utf8_bin;