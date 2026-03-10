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

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ 'c8635278-2e63-11f0-b46c-1aebcda1d33f:1-3675';

--
-- Table structure for table `fakulteler`
--

DROP TABLE IF EXISTS `fakulteler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fakulteler` (
  `fakulte_uuid` varchar(36) NOT NULL DEFAULT (uuid()),
  `fakulte_ad` varchar(150) NOT NULL,
  `web_adres` varchar(200) NOT NULL,
  `kurulus_tarihi` date NOT NULL,
  `olusturma_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `guncelleme_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`fakulte_uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fakulteler`
--

LOCK TABLES `fakulteler` WRITE;
/*!40000 ALTER TABLE `fakulteler` DISABLE KEYS */;
INSERT INTO `fakulteler` VALUES ('66666666-d1cc-11f0-9e59-2e6112e38707','Mühendislik Fakültesi','muhendislik.ahievran.edu.tr','1995-09-01','2026-03-01 18:46:08','2026-03-01 18:46:08'),('77777777-d1cc-11f0-9e59-2e6112e38707','Fen Edebiyat Fakültesi','fenedebiyat.ahievran.edu.tr','1980-09-01','2026-03-01 18:46:08','2026-03-01 18:46:08'),('88888888-d1cc-11f0-9e59-2e6112e38707','Tıp Fakültesi','tip.ahievran.edu.tr','2000-09-01','2026-03-01 18:46:08','2026-03-01 18:46:08'),('99999999-d1cc-11f0-9e59-2e6112e38707','Eğitim Fakültesi','egitim.ahievran.edu.tr','1985-09-01','2026-03-01 18:46:08','2026-03-01 18:46:08'),('aaaaaaa1-d1cc-11f0-9e59-2e6112e38707','İktisadi ve İdari Bilimler Fakültesi','iibf.ahievran.edu.tr','1990-09-01','2026-03-01 18:46:08','2026-03-01 18:46:08');
/*!40000 ALTER TABLE `fakulteler` ENABLE KEYS */;
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

-- Dump completed on 2026-03-11  2:55:10
