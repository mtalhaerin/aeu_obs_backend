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
-- Table structure for table `dersler`
--

DROP TABLE IF EXISTS `dersler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dersler` (
  `ders_uuid` varchar(36) NOT NULL DEFAULT (uuid()),
  `ders_kodu` varchar(20) NOT NULL,
  `ders_adi` varchar(100) NOT NULL,
  `aciklama` text,
  `haftalik_ders_saati` int NOT NULL DEFAULT '0',
  `kredi` int NOT NULL DEFAULT '0',
  `akts` int NOT NULL DEFAULT '0',
  `olusturma_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `guncelleme_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`ders_uuid`),
  UNIQUE KEY `ders_kodu` (`ders_kodu`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dersler`
--

LOCK TABLES `dersler` WRITE;
/*!40000 ALTER TABLE `dersler` DISABLE KEYS */;
INSERT INTO `dersler` VALUES ('2e700f8f-0470-4ad7-a0f7-e463afcbc1cd','BLM121','Programlamaya Giriş','Temel programlama kavramları ve C# dili',5,3,6,'2026-03-09 16:11:32','2026-03-09 16:36:55'),('6547c2ce-d1cc-11f0-9e59-2e6112e38707','BLM101','Programlamaya Giriş','Temel programlama kavramları ve C# dili',4,3,9,'2026-03-01 18:46:07','2026-03-09 15:56:14'),('6547c70f-d1cc-11f0-9e59-2e6112e38707','BLM301','Veritabanı Sistemleri','İlişkisel veritabanları ve SQL',3,3,5,'2026-03-01 18:46:07','2026-03-01 18:46:07'),('6547c83c-d1cc-11f0-9e59-2e6112e38707','BLM401','Yazılım Mühendisliği','Yazılım geliştirme süreçleri ve metodolojiler',3,3,5,'2026-03-01 18:46:07','2026-03-01 18:46:07'),('6547c8bf-d1cc-11f0-9e59-2e6112e38707','BLM201','Veri Yapıları','Temel veri yapıları ve algoritmalar',4,3,6,'2026-03-01 18:46:07','2026-03-01 18:46:07'),('66666666-d1cc-11f0-9e59-2e6112e38707','BLM501','Yapay Zeka','Makine öğrenmesi ve yapay zeka algoritmaları',3,3,5,'2026-03-01 18:46:09','2026-03-01 18:46:09'),('77777777-d1cc-11f0-9e59-2e6112e38707','BLM601','Siber Güvenlik','Bilgi güvenliği ve siber tehditler',3,3,5,'2026-03-01 18:46:09','2026-03-01 18:46:09'),('88888888-d1cc-11f0-9e59-2e6112e38707','BLM701','Robotik','Robotik sistemler ve kontrol',3,3,5,'2026-03-01 18:46:09','2026-03-01 18:46:09'),('99999999-d1cc-11f0-9e59-2e6112e38707','BLM801','Blockchain Teknolojileri','Blockchain ve kripto para sistemleri',3,3,5,'2026-03-01 18:46:09','2026-03-01 18:46:09'),('aaaaaaa2-d1cc-11f0-9e59-2e6112e38707','BLM901','Büyük Veri','Büyük veri analitiği ve Hadoop',3,3,5,'2026-03-01 18:46:09','2026-03-01 18:46:09');
/*!40000 ALTER TABLE `dersler` ENABLE KEYS */;
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

-- Dump completed on 2026-03-11  0:43:00
