DROP DATABASE IF EXISTS aeu_obs;
CREATE DATABASE IF NOT EXISTS aeu_obs;
USE aeu_obs;


CREATE TABLE IF NOT EXISTS kullanicilar(
	kullanici_uuid VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    kullanici_tipi ENUM('OGRENCI','AKADEMISYEN','PERSONEL') DEFAULT 'OGRENCI' NOT NULL,
    ad VARCHAR(50) NOT NULL,
    orta_ad VARCHAR(50),
    soyad VARCHAR(50) NOT NULL,
    kurum_eposta VARCHAR(100) UNIQUE NOT NULL,
    kurum_sicil_no VARCHAR(20) UNIQUE NOT NULL,
    
    parola_hash VARCHAR(255) NOT NULL,
    parola_tuz VARCHAR(64) NOT NULL,
    
	CHECK (kurum_eposta LIKE '%@ahievran.edu.tr'),
    olusturma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    guncelleme_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS adresler(
	adres_uuid VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    kullanici_uuid VARCHAR(36) NOT NULL,
    sokak VARCHAR(100) NOT NULL,
    sehir VARCHAR(50) NOT NULL,
    ilce VARCHAR(50) NOT NULL,
    posta_kodu VARCHAR(10) NOT NULL,
    ulke VARCHAR(50) NOT NULL,
    oncelikli BOOLEAN DEFAULT FALSE NOT NULL,
    olusturma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    guncelleme_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (kullanici_uuid) REFERENCES kullanicilar(kullanici_uuid) ON DELETE CASCADE,
    UNIQUE KEY uk_oncelikli_adres (kullanici_uuid,oncelikli)

);

CREATE TABLE IF NOT EXISTS telefonlar(
	telefon_uuid VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    kullanici_uuid VARCHAR(36) NOT NULL,
    ulke_kodu VARCHAR(5) NOT NULL,
    telefon_numarasi VARCHAR(15) NOT NULL,
    telefon_tipi ENUM('CEP','EV','IS','DIGER') DEFAULT 'CEP',
    oncelikli BOOLEAN DEFAULT FALSE NOT NULL,
    olusturma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    guncelleme_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (kullanici_uuid) REFERENCES kullanicilar(kullanici_uuid) ON DELETE CASCADE,
    UNIQUE (kullanici_uuid, telefon_numarasi),
    UNIQUE KEY uk_oncelikli_telefon(kullanici_uuid,oncelikli)

);

CREATE TABLE IF NOT EXISTS epostalar(
	eposta_uuid VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    kullanici_uuid VARCHAR(36) NOT NULL,
    eposta_adresi VARCHAR(100) UNIQUE NOT NULL,
    eposta_tipi ENUM('KISISEL','IS','DIGER') DEFAULT 'KISISEL' NOT NULL,
    oncelikli BOOLEAN DEFAULT FALSE NOT NULL,
    olusturma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    guncelleme_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (kullanici_uuid) REFERENCES kullanicilar(kullanici_uuid) ON DELETE CASCADE,
    UNIQUE KEY uk_oncelikli_eposta (kullanici_uuid, oncelikli)
);

CREATE TABLE IF NOT EXISTS dersler(
    ders_uuid VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    ders_kodu VARCHAR(20) UNIQUE NOT NULL,
    ders_adi VARCHAR(100) NOT NULL,
    aciklama TEXT,
    haftalik_ders_saati INT DEFAULT 0 NOT NULL,
    kredi INT DEFAULT 0 NOT NULL,
    akts INT DEFAULT 0 NOT NULL,
    olusturma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    guncelleme_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP

);

CREATE TABLE IF NOT EXISTS akademisyen_ders_atamalari(
	atama_uuid VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    akademisyen_uuid VARCHAR(36) NOT NULL,
    ders_uuid VARCHAR(36) NOT NULL,
    olusturma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    guncelleme_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (akademisyen_uuid) REFERENCES kullanicilar(kullanici_uuid) ON DELETE CASCADE,
    FOREIGN KEY (ders_uuid) REFERENCES dersler(ders_uuid) ON DELETE CASCADE,
    UNIQUE (akademisyen_uuid,ders_uuid)
    
);

CREATE TABLE IF NOT EXISTS ogrenci_ders_kayitlari(
	kayit_uuid VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    ogrenci_uuid VARCHAR(36) NOT NULL,
    ders_uuid VARCHAR(36) NOT NULL,
    durum ENUM('DEVAMEDIYOR','GECTI','KALDI') DEFAULT 'DEVAMEDIYOR' NOT NULL,
	olusturma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    guncelleme_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (ogrenci_uuid) REFERENCES kullanicilar(kullanici_uuid) ON DELETE CASCADE,
    FOREIGN KEY (ders_uuid) REFERENCES dersler(ders_uuid) ON DELETE CASCADE,
    UNIQUE (ogrenci_uuid, ders_uuid)
);

CREATE TABLE IF NOT EXISTS sinavlar(
	sinav_uuid VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    ders_uuid VARCHAR(36) NOT NULL,
    sinav_tipi ENUM('QUIZ','VIZE','FINAL','PROJE','BUTUNLEME') DEFAULT 'QUIZ' NOT NULL,
    sinav_tarihi DATE DEFAULT (UTC_DATE()) NOT NULL,
    toplam_puan INT DEFAULT 100 NOT NULL,
    sinav_agirligi DECIMAL(5,2) DEFAULT 0.00 NOT NULL,
    olusturma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    guncelleme_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (ders_uuid) REFERENCES dersler(ders_uuid) ON DELETE CASCADE
    
);

CREATE TABLE IF NOT EXISTS notlar(
	not_uuid VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    sinav_uuid VARCHAR(36) NOT NULL,
    ogrenci_uuid VARCHAR(36) NOT NULL,
    alinan_puan INT DEFAULT 0 NOT NULL,
    olusturma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    guncelleme_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (sinav_uuid) REFERENCES sinavlar(sinav_uuid) ON DELETE CASCADE,
    FOREIGN KEY (ogrenci_uuid) REFERENCES kullanicilar(kullanici_uuid) ON DELETE CASCADE,
    UNIQUE (sinav_uuid, ogrenci_uuid)

);







INSERT INTO kullanicilar (kullanici_uuid, kullanici_tipi, ad, soyad, kurum_eposta, kurum_sicil_no, parola_hash, parola_tuz)
VALUES 
    ('6522f71a-d1cc-11f0-9e59-2e6112e38707', 'AKADEMISYEN', 'Ahmet', 'Yılmaz', 'ahmet.yilmaz@ahievran.edu.tr', 'AKD001', 'hash123', 'salt123'),
    ('6522fbca-d1cc-11f0-9e59-2e6112e38707', 'AKADEMISYEN', 'Fatma', 'Demir', 'fatma.demir@ahievran.edu.tr', 'AKD002', 'hash456', 'salt456');

-- Örnek Öğrenciler
INSERT INTO kullanicilar (kullanici_uuid, kullanici_tipi, ad, soyad, kurum_eposta, kurum_sicil_no, parola_hash, parola_tuz)
VALUES 
    ('653479ae-d1cc-11f0-9e59-2e6112e38707', 'OGRENCI', 'Mehmet', 'Kaya', 'mehmet.kaya@ahievran.edu.tr', '2020001', 'hash789', 'salt789'),
    ('653482ab-d1cc-11f0-9e59-2e6112e38707', 'OGRENCI', 'Ayşe', 'Çelik', 'ayse.celik@ahievran.edu.tr', '2020002', 'hashabc', 'saltabc'),
    ('6534845e-d1cc-11f0-9e59-2e6112e38707', 'OGRENCI', 'Ali', 'Öztürk', 'ali.ozturk@ahievran.edu.tr', '2020003', 'hashdef', 'saltdef');

-- Örnek Dersler
INSERT INTO dersler (ders_uuid, ders_kodu, ders_adi, aciklama, haftalik_ders_saati, kredi, akts)
VALUES 
    ('6547c2ce-d1cc-11f0-9e59-2e6112e38707', 'BLM101', 'Programlamaya Giriş', 'Temel programlama kavramları ve C# dili', 4, 3, 5),
    ('6547c8bf-d1cc-11f0-9e59-2e6112e38707', 'BLM201', 'Veri Yapıları', 'Temel veri yapıları ve algoritmalar', 4, 3, 6),
    ('6547c70f-d1cc-11f0-9e59-2e6112e38707', 'BLM301', 'Veritabanı Sistemleri', 'İlişkisel veritabanları ve SQL', 3, 3, 5),
    ('6547c83c-d1cc-11f0-9e59-2e6112e38707', 'BLM401', 'Yazılım Mühendisliği', 'Yazılım geliştirme süreçleri ve metodolojiler', 3, 3, 5);


-- 1. ADRESLER EKLEME (Öğrenci ve Akademisyenler için)
-- Ahmet Yılmaz (Akademisyen) ve Mehmet Kaya (Öğrenci) için adresler
INSERT INTO adresler (kullanici_uuid, sokak, sehir, ilce, posta_kodu, ulke, oncelikli) VALUES
('6522f71a-d1cc-11f0-9e59-2e6112e38707', 'Ahi Evran Bulvarı No:14', 'Kırşehir', 'Merkez', '40100', 'Türkiye', 1),
('653479ae-d1cc-11f0-9e59-2e6112e38707', 'Kuşdili Mah. 12. Sokak', 'Kırşehir', 'Merkez', '40200', 'Türkiye', 1),
('653482ab-d1cc-11f0-9e59-2e6112e38707', 'Bahçelievler 7. Cadde', 'Ankara', 'Çankaya', '06500', 'Türkiye', 1);

-- 2. TELEFONLAR EKLEME
INSERT INTO telefonlar (kullanici_uuid, ulke_kodu, telefon_numarasi, telefon_tipi, oncelikli) VALUES
('6522f71a-d1cc-11f0-9e59-2e6112e38707', '+90', '5551112233', 'CEP', 1), -- Ahmet Hoca
('653479ae-d1cc-11f0-9e59-2e6112e38707', '+90', '5051234567', 'CEP', 1), -- Mehmet
('653482ab-d1cc-11f0-9e59-2e6112e38707', '+90', '5329876543', 'CEP', 1); -- Ayşe

-- 3. EKSTRA E-POSTALAR (Kişisel mailler)
INSERT INTO epostalar (kullanici_uuid, eposta_adresi, eposta_tipi, oncelikli) VALUES
('653479ae-d1cc-11f0-9e59-2e6112e38707', 'mehmet.kaya@gmail.com', 'KISISEL', 1),
('6522f71a-d1cc-11f0-9e59-2e6112e38707', 'ahmetyilmaz@hotmail.com', 'KISISEL', 1);

-- 4. AKADEMİSYEN DERS ATAMALARI
-- Ahmet Yılmaz -> Programlamaya Giriş (BLM101) ve Veritabanı (BLM301)
-- Fatma Demir -> Veri Yapıları (BLM201) ve Yazılım Müh. (BLM401)
INSERT INTO akademisyen_ders_atamalari (akademisyen_uuid, ders_uuid) VALUES
('6522f71a-d1cc-11f0-9e59-2e6112e38707', '6547c2ce-d1cc-11f0-9e59-2e6112e38707'), -- Ahmet -> BLM101
('6522f71a-d1cc-11f0-9e59-2e6112e38707', '6547c70f-d1cc-11f0-9e59-2e6112e38707'), -- Ahmet -> BLM301
('6522fbca-d1cc-11f0-9e59-2e6112e38707', '6547c8bf-d1cc-11f0-9e59-2e6112e38707'), -- Fatma -> BLM201
('6522fbca-d1cc-11f0-9e59-2e6112e38707', '6547c83c-d1cc-11f0-9e59-2e6112e38707'); -- Fatma -> BLM401

-- 5. ÖĞRENCİ DERS KAYITLARI
-- Mehmet: BLM101 ve BLM201 alıyor
-- Ayşe: BLM101 ve BLM301 alıyor
-- Ali: Sadece BLM401 alıyor
INSERT INTO ogrenci_ders_kayitlari (ogrenci_uuid, ders_uuid, durum) VALUES
('653479ae-d1cc-11f0-9e59-2e6112e38707', '6547c2ce-d1cc-11f0-9e59-2e6112e38707', 'DEVAMEDIYOR'), -- Mehmet -> BLM101
('653479ae-d1cc-11f0-9e59-2e6112e38707', '6547c8bf-d1cc-11f0-9e59-2e6112e38707', 'DEVAMEDIYOR'), -- Mehmet -> BLM201
('653482ab-d1cc-11f0-9e59-2e6112e38707', '6547c2ce-d1cc-11f0-9e59-2e6112e38707', 'DEVAMEDIYOR'), -- Ayşe -> BLM101
('653482ab-d1cc-11f0-9e59-2e6112e38707', '6547c70f-d1cc-11f0-9e59-2e6112e38707', 'DEVAMEDIYOR'), -- Ayşe -> BLM301
('6534845e-d1cc-11f0-9e59-2e6112e38707', '6547c83c-d1cc-11f0-9e59-2e6112e38707', 'DEVAMEDIYOR'); -- Ali -> BLM401

-- 6. SINAVLAR OLUŞTURMA
-- Not girebilmek için manuel UUID vererek sınav oluşturuyoruz.
-- BLM101 (Programlamaya Giriş) için Vize ve Final
-- BLM401 (Yazılım Müh) için Proje
INSERT INTO sinavlar (sinav_uuid, ders_uuid, sinav_tipi, sinav_tarihi, toplam_puan, sinav_agirligi) VALUES
('aaaaaaaa-1111-1111-1111-111111111111', '6547c2ce-d1cc-11f0-9e59-2e6112e38707', 'VIZE', '2025-11-10', 100, 40.00),
('bbbbbbbb-2222-2222-2222-222222222222', '6547c2ce-d1cc-11f0-9e59-2e6112e38707', 'FINAL', '2026-01-15', 100, 60.00),
('cccccccc-3333-3333-3333-333333333333', '6547c83c-d1cc-11f0-9e59-2e6112e38707', 'PROJE', '2025-12-20', 100, 100.00);

-- 7. NOT GİRİŞLERİ
-- Mehmet ve Ayşe'nin BLM101 Vize ve Final notları
-- Ali'nin BLM401 Proje notu

-- Mehmet (BLM101) Vize: 55, Final: 70
INSERT INTO notlar (sinav_uuid, ogrenci_uuid, alinan_puan) VALUES
('aaaaaaaa-1111-1111-1111-111111111111', '653479ae-d1cc-11f0-9e59-2e6112e38707', 55),
('bbbbbbbb-2222-2222-2222-222222222222', '653479ae-d1cc-11f0-9e59-2e6112e38707', 70);

-- Ayşe (BLM101) Vize: 85, Final: 90
INSERT INTO notlar (sinav_uuid, ogrenci_uuid, alinan_puan) VALUES
('aaaaaaaa-1111-1111-1111-111111111111', '653482ab-d1cc-11f0-9e59-2e6112e38707', 85),
('bbbbbbbb-2222-2222-2222-222222222222', '653482ab-d1cc-11f0-9e59-2e6112e38707', 90);

-- Ali (BLM401) Proje: 100
INSERT INTO notlar (sinav_uuid, ogrenci_uuid, alinan_puan) VALUES
('cccccccc-3333-3333-3333-333333333333', '6534845e-d1cc-11f0-9e59-2e6112e38707', 100);


SELECT 
    k.ad, 
    k.soyad, 
    d.ders_adi, 
    s.sinav_tipi, 
    n.alinan_puan 
FROM notlar n
JOIN sinavlar s ON n.sinav_uuid = s.sinav_uuid
JOIN dersler d ON s.ders_uuid = d.ders_uuid
JOIN kullanicilar k ON n.ogrenci_uuid = k.kullanici_uuid;