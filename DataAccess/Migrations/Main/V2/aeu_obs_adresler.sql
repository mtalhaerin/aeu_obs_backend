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
-- Table structure for table `adresler`
--

DROP TABLE IF EXISTS `adresler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `adresler` (
  `adres_uuid` varchar(36) NOT NULL DEFAULT (uuid()),
  `kullanici_uuid` varchar(36) NOT NULL,
  `sokak` varchar(100) NOT NULL,
  `sehir` varchar(50) NOT NULL,
  `ilce` varchar(50) NOT NULL,
  `posta_kodu` varchar(10) NOT NULL,
  `ulke` varchar(50) NOT NULL,
  `oncelikli` tinyint(1) NOT NULL DEFAULT '0',
  `olusturma_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `guncelleme_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`adres_uuid`),
  KEY `adresler_ibfk_1_idx` (`kullanici_uuid`),
  CONSTRAINT `adresler_ibfk_1` FOREIGN KEY (`kullanici_uuid`) REFERENCES `kullanicilar` (`kullanici_uuid`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `adresler`
--

LOCK TABLES `adresler` WRITE;
/*!40000 ALTER TABLE `adresler` DISABLE KEYS */;
INSERT INTO `adresler` VALUES ('352e6240-3f91-4aed-8134-c284904a7032','11111111-d1cc-11f0-9e59-2e6112e38707','a1223','23123','12312','31231','231',0,'2026-03-04 15:36:34','2026-03-04 15:36:34'),('75e31f23-7a26-4ac6-928d-0c235b73a071','11111111-d1cc-11f0-9e59-2e6112e38707','111111','asdas','dasdas','dsad','sada',0,'2026-03-04 13:59:44','2026-03-04 13:59:51'),('e89b7449-159e-11f1-b519-2e6112e38707','6522f71a-d1cc-11f0-9e59-2e6112e38707','Ahi Evran Bulvarı No:14','Kırşehir','Merkez','40100','Türkiye',1,'2026-03-01 18:46:07','2026-03-01 18:46:07'),('e89b7820-159e-11f1-b519-2e6112e38707','653479ae-d1cc-11f0-9e59-2e6112e38707','Kuşdili Mah. 12. Sokak','Kırşehir','Merkez','40200','Türkiye',1,'2026-03-01 18:46:07','2026-03-01 18:46:07'),('e89b7aa9-159e-11f1-b519-2e6112e38707','653482ab-d1cc-11f0-9e59-2e6112e38707','Bahçelievler 7. Cadde','Ankara','Çankaya','06500','Türkiye',1,'2026-03-01 18:46:07','2026-03-01 18:46:07'),('e98c8ff0-159e-11f1-b519-2e6112e38707','11111111-d1cc-11f0-9e59-2e6112e38707','Atatürk Cad. No:5','İstanbul','Kadıköy','34710','Türkiye',1,'2026-03-01 18:46:09','2026-03-01 18:46:09'),('e98c969a-159e-11f1-b519-2e6112e38707','33333333-d1cc-11f0-9e59-2e6112e38707','Barbaros Bulvarı No:20','Ankara','Çankaya','06530','Türkiye',1,'2026-03-01 18:46:09','2026-03-01 18:46:09'),('e98c9725-159e-11f1-b519-2e6112e38707','44444444-d1cc-11f0-9e59-2e6112e38707','İnönü Cad. No:15','Antalya','Muratpaşa','07100','Türkiye',1,'2026-03-01 18:46:09','2026-03-01 18:46:09'),('e98c97a9-159e-11f1-b519-2e6112e38707','55555555-d1cc-11f0-9e59-2e6112e38707','Kordonboyu Mah. 7. Sokak','Trabzon','Ortahisar','61030','Türkiye',1,'2026-03-01 18:46:09','2026-03-01 18:46:09');
/*!40000 ALTER TABLE `adresler` ENABLE KEYS */;
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

-- Dump completed on 2026-03-11  0:43:13
