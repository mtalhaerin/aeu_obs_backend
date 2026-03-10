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
-- Table structure for table `kullanicilar`
--

DROP TABLE IF EXISTS `kullanicilar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kullanicilar` (
  `kullanici_uuid` varchar(36) NOT NULL DEFAULT (uuid()),
  `kullanici_tipi` enum('OGRENCI','AKADEMISYEN','PERSONEL') NOT NULL DEFAULT 'OGRENCI',
  `ad` varchar(50) NOT NULL,
  `orta_ad` varchar(50) DEFAULT NULL,
  `soyad` varchar(50) NOT NULL,
  `kurum_eposta` varchar(100) NOT NULL,
  `kurum_sicil_no` varchar(20) NOT NULL,
  `parola_hash` varchar(255) NOT NULL,
  `parola_tuz` varchar(255) NOT NULL,
  `olusturma_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `guncelleme_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`kullanici_uuid`),
  UNIQUE KEY `kurum_eposta` (`kurum_eposta`),
  UNIQUE KEY `kurum_sicil_no` (`kurum_sicil_no`),
  CONSTRAINT `kullanicilar_chk_1` CHECK ((`kurum_eposta` like _utf8mb4'%@ahievran.edu.tr'))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kullanicilar`
--

LOCK TABLES `kullanicilar` WRITE;
/*!40000 ALTER TABLE `kullanicilar` DISABLE KEYS */;
INSERT INTO `kullanicilar` VALUES ('11111111-d1cc-11f0-9e59-2e6112e38707','OGRENCI','Zeynep',NULL,'Kara','zeynep.kara@ahievran.edu.tr','2020004','2mNt2Qj4z7fiJIr7hTgFpULN6JWRQdwCvz5d4HZ60Mk=','5lcuF3NnFPDpXMilFBzmkVl1Vd+pjd+6Fx5PBDNEVd4PvZDed7Ygf94J0ELG9wL/j06WocE2lflfH+foX5/oBw==','2026-03-01 18:46:08','2026-03-02 07:58:54'),('33333333-d1cc-11f0-9e59-2e6112e38707','AKADEMISYEN','Selin',NULL,'Aydın','selin.aydin@ahievran.edu.tr','AKD003','2mNt2Qj4z7fiJIr7hTgFpULN6JWRQdwCvz5d4HZ60Mk=','5lcuF3NnFPDpXMilFBzmkVl1Vd+pjd+6Fx5PBDNEVd4PvZDed7Ygf94J0ELG9wL/j06WocE2lflfH+foX5/oBw==','2026-03-01 18:46:08','2026-03-02 07:58:54'),('44444444-d1cc-11f0-9e59-2e6112e38707','PERSONEL','Murat',NULL,'Işık','murat.isik@ahievran.edu.tr','PRS001','2mNt2Qj4z7fiJIr7hTgFpULN6JWRQdwCvz5d4HZ60Mk=','5lcuF3NnFPDpXMilFBzmkVl1Vd+pjd+6Fx5PBDNEVd4PvZDed7Ygf94J0ELG9wL/j06WocE2lflfH+foX5/oBw==','2026-03-01 18:46:08','2026-03-05 13:04:42'),('55555555-d1cc-11f0-9e59-2e6112e38707','PERSONEL','Elif',NULL,'Demir','elif.demir@ahievran.edu.tr','PRS002','2mNt2Qj4z7fiJIr7hTgFpULN6JWRQdwCvz5d4HZ60Mk=','5lcuF3NnFPDpXMilFBzmkVl1Vd+pjd+6Fx5PBDNEVd4PvZDed7Ygf94J0ELG9wL/j06WocE2lflfH+foX5/oBw==','2026-03-01 18:46:08','2026-03-02 07:58:55'),('6522f71a-d1cc-11f0-9e59-2e6112e38707','AKADEMISYEN','Ahmet',NULL,'Yılmaz','ahmet.yilmaz@ahievran.edu.tr','AKD001','2mNt2Qj4z7fiJIr7hTgFpULN6JWRQdwCvz5d4HZ60Mk=','5lcuF3NnFPDpXMilFBzmkVl1Vd+pjd+6Fx5PBDNEVd4PvZDed7Ygf94J0ELG9wL/j06WocE2lflfH+foX5/oBw==','2026-03-01 18:46:07','2026-03-02 07:58:55'),('6522fbca-d1cc-11f0-9e59-2e6112e38707','AKADEMISYEN','Fatma',NULL,'Demir','fatma.demir@ahievran.edu.tr','AKD002','2mNt2Qj4z7fiJIr7hTgFpULN6JWRQdwCvz5d4HZ60Mk=','5lcuF3NnFPDpXMilFBzmkVl1Vd+pjd+6Fx5PBDNEVd4PvZDed7Ygf94J0ELG9wL/j06WocE2lflfH+foX5/oBw==','2026-03-01 18:46:07','2026-03-02 07:58:55'),('653479ae-d1cc-11f0-9e59-2e6112e38707','OGRENCI','Mehmet',NULL,'Kaya','mehmet.kaya@ahievran.edu.tr','2020001','2mNt2Qj4z7fiJIr7hTgFpULN6JWRQdwCvz5d4HZ60Mk=','5lcuF3NnFPDpXMilFBzmkVl1Vd+pjd+6Fx5PBDNEVd4PvZDed7Ygf94J0ELG9wL/j06WocE2lflfH+foX5/oBw==','2026-03-01 18:46:07','2026-03-02 07:58:56'),('653482ab-d1cc-11f0-9e59-2e6112e38707','OGRENCI','Ayşe',NULL,'Çelik','ayse.celik@ahievran.edu.tr','2020002','2mNt2Qj4z7fiJIr7hTgFpULN6JWRQdwCvz5d4HZ60Mk=','5lcuF3NnFPDpXMilFBzmkVl1Vd+pjd+6Fx5PBDNEVd4PvZDed7Ygf94J0ELG9wL/j06WocE2lflfH+foX5/oBw==','2026-03-01 18:46:07','2026-03-02 07:58:56');
/*!40000 ALTER TABLE `kullanicilar` ENABLE KEYS */;
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

-- Dump completed on 2026-03-11  2:55:26
