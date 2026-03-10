-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: 100.64.207.39    Database: aeu_obs
-- ------------------------------------------------------
-- Server version	9.5.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ 'c8635278-2e63-11f0-b46c-1aebcda1d33f:1-3650';

--
-- Table structure for table `ana_dallar`
--

DROP TABLE IF EXISTS `ana_dallar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ana_dallar` (
  `ana_dal_uuid` varchar(36) NOT NULL DEFAULT (uuid()),
  `ana_dal_ad` varchar(150) NOT NULL,
  `fakulte_uuid` varchar(36) NOT NULL,
  `kurulus_tarihi` date NOT NULL,
  `olusturma_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `guncelleme_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`ana_dal_uuid`),
  KEY `ana_dallar_ibfk_1` (`fakulte_uuid`),
  CONSTRAINT `ana_dallar_ibfk_1` FOREIGN KEY (`fakulte_uuid`) REFERENCES `fakulteler` (`fakulte_uuid`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ana_dallar`
--

LOCK TABLES `ana_dallar` WRITE;
/*!40000 ALTER TABLE `ana_dallar` DISABLE KEYS */;
INSERT INTO `ana_dallar` VALUES ('bbbbbbb1-d1cc-11f0-9e59-2e6112e38707','Yazılım','66666666-d1cc-11f0-9e59-2e6112e38707','1995-09-01','2026-03-01 18:46:09','2026-03-10 21:39:30'),('ccccccc1-d1cc-11f0-9e59-2e6112e38707','Veri Bilimi','66666666-d1cc-11f0-9e59-2e6112e38707','1995-09-01','2026-03-01 18:46:09','2026-03-10 21:39:30'),('ddddddd1-d1cc-11f0-9e59-2e6112e38707','Termodinamik','77777777-d1cc-11f0-9e59-2e6112e38707','1980-09-01','2026-03-01 18:46:09','2026-03-10 21:39:30'),('eeeeeee1-d1cc-11f0-9e59-2e6112e38707','Cebir','77777777-d1cc-11f0-9e59-2e6112e38707','1980-09-01','2026-03-01 18:46:09','2026-03-10 21:39:30'),('fffffff1-d1cc-11f0-9e59-2e6112e38707','Quant','88888888-d1cc-11f0-9e59-2e6112e38707','2000-09-01','2026-03-01 18:46:09','2026-03-10 21:39:31');
/*!40000 ALTER TABLE `ana_dallar` ENABLE KEYS */;
UNLOCK TABLES;
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-11  0:43:36
