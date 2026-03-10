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
-- Table structure for table `epostalar`
--

DROP TABLE IF EXISTS `epostalar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `epostalar` (
  `eposta_uuid` varchar(36) NOT NULL DEFAULT (uuid()),
  `kullanici_uuid` varchar(36) NOT NULL,
  `eposta_adresi` varchar(100) NOT NULL,
  `eposta_tipi` enum('KISISEL','IS','DIGER') NOT NULL DEFAULT 'KISISEL',
  `oncelikli` tinyint(1) NOT NULL DEFAULT '0',
  `olusturma_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `guncelleme_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`eposta_uuid`),
  KEY `epostalar_ibfk_1` (`kullanici_uuid`),
  CONSTRAINT `epostalar_ibfk_1` FOREIGN KEY (`kullanici_uuid`) REFERENCES `kullanicilar` (`kullanici_uuid`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `epostalar`
--

LOCK TABLES `epostalar` WRITE;
/*!40000 ALTER TABLE `epostalar` DISABLE KEYS */;
INSERT INTO `epostalar` VALUES ('1a414aa9-45f8-4498-ab99-2cd2d271e72c','11111111-d1cc-11f0-9e59-2e6112e38707','asdasd','KISISEL',0,'2026-03-04 14:11:01','2026-03-04 14:11:01'),('e8c01b10-159e-11f1-b519-2e6112e38707','653479ae-d1cc-11f0-9e59-2e6112e38707','mehmet.kaya@gmail.com','KISISEL',1,'2026-03-01 18:46:07','2026-03-01 18:46:07'),('e8c01f37-159e-11f1-b519-2e6112e38707','6522f71a-d1cc-11f0-9e59-2e6112e38707','ahmetyilmaz@hotmail.com','KISISEL',1,'2026-03-01 18:46:07','2026-03-01 18:46:07'),('e9af37a9-159e-11f1-b519-2e6112e38707','11111111-d1cc-11f0-9e59-2e6112e38707','zeynep.kara@gmail.com','KISISEL',1,'2026-03-01 18:46:09','2026-03-01 18:46:09'),('e9af40d5-159e-11f1-b519-2e6112e38707','33333333-d1cc-11f0-9e59-2e6112e38707','selin.aydin@gmail.com','KISISEL',1,'2026-03-01 18:46:09','2026-03-01 18:46:09'),('e9af41d7-159e-11f1-b519-2e6112e38707','44444444-d1cc-11f0-9e59-2e6112e38707','murat.yildiz@gmail.com','KISISEL',1,'2026-03-01 18:46:09','2026-03-01 18:46:09'),('e9af42db-159e-11f1-b519-2e6112e38707','55555555-d1cc-11f0-9e59-2e6112e38707','elif.demir@gmail.com','KISISEL',1,'2026-03-01 18:46:09','2026-03-01 18:46:09');
/*!40000 ALTER TABLE `epostalar` ENABLE KEYS */;
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

-- Dump completed on 2026-03-11  2:55:13
